using System.Linq;
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
            agent.Run();
            Assert.True(agent.IsRunning);

            agent.Idle();
            Assert.False(agent.IsRunning);
        }

        [Fact]
        public void Run_should_enable_skills()
        {
            var source = new BufferBlock<IMessage>();
            var target = new BufferBlock<IMessage>();

            var skill1 = new DataflowSkillFake();
            skill1.SetSource(source);
            skill1.SetTarget(target);

            var skill2 = new DataflowSkillFake();
            skill2.SetSource(source);
            skill2.SetTarget(target);

            var skill3 = new DataflowSkillFake();
            skill3.SetSource(source);
            skill3.SetTarget(target);

            var skills = new[] { skill1, skill2, skill3 };

            var agent = new DataflowAgentFake(skills);
            agent.Run();

            Assert.All(skills, s => Assert.True(s.IsEnabled));
        }
    }
}
