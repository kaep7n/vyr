using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Playground.Agents
{
    public class CoreAgent : DataflowAgent
    {
        public CoreAgent(IEnumerable<ISkill> skills) 
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
