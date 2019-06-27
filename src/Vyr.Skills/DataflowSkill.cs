using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {
        private readonly ITargetBlock<IMessage> incomingTargetBlock;
        private readonly BufferBlock<IMessage> incomingBlock = new BufferBlock<IMessage>();
        private readonly BroadcastBlock<IMessage> outgoingBlock = new BroadcastBlock<IMessage>(i => i);

        private IDisposable incomingBlockLink;

        public DataflowSkill()
        {
            this.incomingTargetBlock = new ActionBlock<IMessage>(this.ProcessAsync);
        }

        public bool IsEnabled { get; private set; }

        public string Topic { get; private set; }

        public void Enable()
        {
            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock, new DataflowLinkOptions { PropagateCompletion = true });
            this.IsEnabled = true;
        }

        public async Task EnqueueAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (!this.IsEnabled)
            {
                return;
            }

            await this.incomingBlock.SendAsync(message);
        }

        public void Subscribe(Action<IMessage> message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.outgoingBlock.LinkTo(new ActionBlock<IMessage>(message));
        }

        public void Disable()
        {
            this.incomingBlockLink.Dispose();
            this.IsEnabled = false;
        }

        protected async Task PublishAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.outgoingBlock.SendAsync(message);
        }

        protected virtual Task ProcessAsync(IMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
