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
        private readonly BufferBlock<IMessage> outgoingBlock = new BufferBlock<IMessage>();
        private readonly ISourceBlock<IMessage> sourceBlock;

        private IDisposable sourceBlockLink;
        private IDisposable incomingBlockLink;

        public DataflowSkill(ISourceBlock<IMessage> source)
        {
            this.incomingTargetBlock = new ActionBlock<IMessage>(this.ProcessAsync);
            this.sourceBlock = source;
        }

        public bool IsEnabled { get; private set; }

        public string[] AcceptedTopics { get; private set; }

        public void Enable()
        {
            this.sourceBlockLink = this.sourceBlock.LinkTo(this.incomingBlock);

            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock);
            this.IsEnabled = true;
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
            this.sourceBlockLink.Dispose();
            this.sourceBlockLink = null;

            this.incomingBlockLink.Dispose();
            this.incomingBlockLink = null;

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
