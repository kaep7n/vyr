using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {
        private readonly BufferBlock<IMessage> incomingBuffer = new BufferBlock<IMessage>();
        private readonly ITargetBlock<IMessage> incomingTargetBlock;

        private readonly BufferBlock<IMessage> outgoingBuffer = new BufferBlock<IMessage>();

        private ISourceBlock<IMessage> sourceBlock;
        private ITargetBlock<IMessage> targetBlock;

        private IDisposable sourceBlockLink;
        private IDisposable targetBlockLink;
        private IDisposable incomingBlockLink;

        public DataflowSkill()
        {
            this.incomingTargetBlock = new ActionBlock<IMessage>(this.ProcessAsync);
        }

        public bool IsEnabled { get; private set; }

        public IEnumerable<string> Topics => this.GetTopics();

        public void Enable()
        {
            this.sourceBlockLink = this.sourceBlock.LinkTo(this.incomingBuffer, m => this.Topics.Contains(m.Topic));
            this.targetBlockLink = this.outgoingBuffer.LinkTo(this.targetBlock);
            this.incomingBlockLink = this.incomingBuffer.LinkTo(this.incomingTargetBlock);

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

        public void SetSource(ISourceBlock<IMessage> sourceBlock)
        {
            if (this.IsEnabled)
            {
                throw new InvalidOperationException("Source can only be set then skill is not enabled");
            }

            this.sourceBlock = sourceBlock;
        }

        public void SetTarget(ITargetBlock<IMessage> targetBlock)
        {
            if (this.IsEnabled)
            {
                throw new InvalidOperationException("Target can only be set then skill is not enabled");
            }

            this.targetBlock = targetBlock;
        }

        protected async Task PublishAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.outgoingBuffer.SendAsync(message);
        }

        protected virtual Task ProcessAsync(IMessage message)
        {
            return Task.CompletedTask;
        }

        protected virtual IEnumerable<string> GetTopics()
        {
            return Enumerable.Empty<string>();
        }
    }
}
