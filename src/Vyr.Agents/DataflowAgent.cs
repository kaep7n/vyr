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
                skill.Subscribe(this.ProcessMessage);
            }

            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock);
            this.IsRunning = true;
        }

        public async Task EnqueueAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.incomingBlock.SendAsync(message);
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
