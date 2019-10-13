using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Playground.Agents
{
    public class TestAgent : DataflowAgent
    {
        public TestAgent(IEnumerable<ISkill> skills) 
            : base(skills)
        {
        }

        public override Task RunAsync()
        {
            return base.RunAsync();
        }

        public override Task IdleAsync()
        {
            return base.IdleAsync();
        }

        protected override void ProcessMessage(IMessage message)
        {
            base.ProcessMessage(message);
        }
    }
}
