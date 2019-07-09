using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Agents
{
    public class DataflowAgent : IAgent
    {
        private readonly IEnumerable<ISkill> skills;

        private readonly BufferBlock<IMessage> incomingBlock = new BufferBlock<IMessage>();
        private readonly ActionBlock<IMessage> incomingTargetBlock;

        private IDisposable incomingBlockLink;

        public DataflowAgent(IEnumerable<ISkill> skills)
        {
            if (skills is null)
            {
                throw new ArgumentNullException(nameof(skills));
            }

            this.skills = skills;
            this.incomingTargetBlock = new ActionBlock<IMessage>(this.ProcessMessage);
        }

        public bool IsRunning { get; private set; }

        public void Run()
        {
            foreach (var skill in this.skills)
            {
                skill.Enable();
            }

            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock);
            this.IsRunning = true;
        }

        protected virtual void ProcessMessage(IMessage message)
        {
        }

        public void Idle()
        {
            this.incomingBlockLink.Dispose();
            this.IsRunning = false;
        }
    }
}
