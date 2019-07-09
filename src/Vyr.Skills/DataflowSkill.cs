using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {

        private readonly BufferBlock<IMessage> incomingBlock = new BufferBlock<IMessage>();
        private readonly ITargetBlock<IMessage> incomingTargetBlock;

        private readonly BufferBlock<IMessage> outgoingBlock = new BufferBlock<IMessage>();

        private readonly ISourceBlock<IMessage> sourceBlock;
        private readonly ITargetBlock<IMessage> targetBlock;

        private IDisposable sourceBlockLink;
        private IDisposable targetBlockLink;
        private IDisposable incomingBlockLink;

        public DataflowSkill(ISourceBlock<IMessage> source, ITargetBlock<IMessage> target)
        {
            this.incomingTargetBlock = new ActionBlock<IMessage>(this.ProcessAsync);
            this.sourceBlock = source;
            this.targetBlock = target;
        }

        public bool IsEnabled { get; private set; }

        public string[] AcceptedTopics { get; private set; }

        public void Enable()
        {
            this.sourceBlockLink = this.sourceBlock.LinkTo(this.incomingBlock);
            this.targetBlockLink = this.outgoingBlock.LinkTo(this.targetBlock);
            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock);

            this.IsEnabled = true;
        }

        public void Disable()
        {
            this.sourceBlockLink.Dispose();
            this.sourceBlockLink = null;

            this.incomingBlockLink.Dispose();
            this.incomingBlockLink = null;

            this.targetBlockLink.Dispose();
            this.targetBlockLink = null;

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
