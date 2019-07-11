using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Agents.Tests.Fakes;
using Vyr.Core;
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
            agent.RunAsync();
            Assert.True(agent.IsRunning);

            agent.IdleAsync();
            Assert.False(agent.IsRunning);
        }

        [Fact]
        public void Run_should_enable_skills()
        {
            var skill1 = new DataflowSkillFake(i => null, "Test1");
            var skill2 = new DataflowSkillFake(i => null, "Test2");
            var skill3 = new DataflowSkillFake(i => null, "Test3");

            var skills = new[] { skill1, skill2, skill3 };

            var agent = new DataflowAgentFake(skills);
            agent.RunAsync();

            Assert.All(skills, s => Assert.True(s.IsEnabled));
        }

        [Fact]
        public async Task Send_message_from_skill_should_send_response_message_to_skill()
        {
            var skill1 = new DataflowSkillFake(i => null);
            var skill2 = new DataflowSkillFake(i => null, "Test1");

            var skills = new[] { skill1, skill2};

            var agent = new DataflowAgentFake(skills);
            agent.RunAsync();

            await skill1.SendAsync(new FakeMessage("Test1"));

            Thread.Sleep(50);

            Assert.Equal(0, skill1.GetProcessedMessagesCount());
            Assert.Equal(1, skill2.GetProcessedMessagesCount());
        }

        [Fact]
        public async Task Send_message_from_skill_should_send_response_message_to_next_skill()
        {
            var skill1 = new DataflowSkillFake(i => null);
            var skill2 = new DataflowSkillFake(i => new FakeMessage("Test2"), "Test1");
            var skill3 = new DataflowSkillFake(i => null, "Test2");

            var skills = new[] { skill1, skill2, skill3 };

            var agent = new DataflowAgentFake(skills);
            agent.RunAsync();

            await skill1.SendAsync(new FakeMessage("Test1"));

            Thread.Sleep(50);

            Assert.Equal(0, skill1.GetProcessedMessagesCount());
            Assert.Equal(1, skill2.GetProcessedMessagesCount());
            Assert.Equal(1, skill3.GetProcessedMessagesCount());
        }

        [Fact]
        public async Task Send_message_from_skill_should_send_message_to_multiple_skills()
        {
            var skill1 = new DataflowSkillFake(i => null);
            var skill2 = new DataflowSkillFake(i => null, "Test1");
            var skill3 = new DataflowSkillFake(i => null, "Test1");
            var skill4 = new DataflowSkillFake(i => null, "Test1");

            var skills = new[] { skill1, skill2, skill3, skill4 };

            var agent = new DataflowAgentFake(skills);
            agent.RunAsync();

            await skill1.SendAsync(new FakeMessage("Test1"));

            Thread.Sleep(50);

            Assert.Equal(0, skill1.GetProcessedMessagesCount());
            Assert.Equal(1, skill2.GetProcessedMessagesCount());
            Assert.Equal(1, skill3.GetProcessedMessagesCount());
            Assert.Equal(1, skill4.GetProcessedMessagesCount());
        }
    }
}
