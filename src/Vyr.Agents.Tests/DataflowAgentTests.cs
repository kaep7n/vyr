using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vyr.Agents.Tests.Fakes;
using Vyr.Skills;
using Vyr.Skills.Tests.Fakes;
using Xunit;

namespace Vyr.Agents.Tests
{
    public class DataflowAgentTests
    {
        [Fact]
        public void Run_and_Idle_should_set_IsRunning()
        {
            var agent = new DataflowAgentFake(Enumerable.Empty<ISkill>());
            agent.Run();
            Assert.True(agent.IsRunning);

            agent.Idle();
            Assert.False(agent.IsRunning);
        }

        [Fact]
        public void Run_should_enable_skills()
        {
            var skills = new[] { new DataflowSkillFake(), new DataflowSkillFake(), new DataflowSkillFake() };

            var agent = new DataflowAgentFake(skills);
            agent.Run();

            Assert.All(skills, s => Assert.True(s.IsEnabled));
        }

        [Fact]
        public async Task EnqueueAsync_should_process_message()
        {
            var agent = new DataflowAgentFake(Enumerable.Empty<ISkill>());
            agent.Run();

            await agent.EnqueueAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.True(agent.HasProcessedAnyMessages());
        }

        [Fact]
        public async Task EnqueueAsync_should_process_message_after_agent_is_idle()
        {
            var agent = new DataflowAgentFake(Enumerable.Empty<ISkill>());

            agent.Run();
            await agent.EnqueueAsync(new FakeMessage());
            Thread.Sleep(10);

            agent.Idle();
            await agent.EnqueueAsync(new FakeMessage());

            // wait for processing (there should be a prettier and reliable solution)
            Thread.Sleep(10);

            Assert.Equal(1, agent.GetProcessedMessagesCount());
        }
    }
}
