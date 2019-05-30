using System.Linq;
using System.Threading.Tasks;
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
            var agent = new DataflowAgent(Enumerable.Empty<ISkill>());
            agent.Run();
            Assert.True(agent.IsRunning);

            agent.Idle();
            Assert.False(agent.IsRunning);
        }

        [Fact]
        public void Run_should_enable_skills()
        {
            var skills = new[]
            {
                new DataflowSkillFake(),
                new DataflowSkillFake(),
                new DataflowSkillFake()
            };

            var agent = new DataflowAgent(skills);
            agent.Run();

            Assert.All(skills, s => Assert.True(s.IsEnabled));
        }
    }
}
