using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {
        private readonly ITargetBlock<IRequest> incomingTargetBlock;
        private readonly BufferBlock<IRequest> incomingBlock = new BufferBlock<IRequest>();
        private readonly BroadcastBlock<IResponse> outgoingBlock = new BroadcastBlock<IResponse>(i => { return i; });

        private IDisposable incomingBlockLink;

        public DataflowSkill()
        {
            this.incomingTargetBlock = new ActionBlock<IRequest>(this.ProcessAsync);
        }

        public bool IsEnabled { get; private set; }

        public string Topic { get; private set; }

        public void Enable()
        {
            this.incomingBlockLink = this.incomingBlock.LinkTo(this.incomingTargetBlock, new DataflowLinkOptions { PropagateCompletion = true });
            this.IsEnabled = true;
        }

        public async Task EnqueueAsync(IRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!this.IsEnabled)
            {
                return;
            }

            await this.incomingBlock.SendAsync(request);
        }

        public void Subscribe(Action<IResponse> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.outgoingBlock.LinkTo(new ActionBlock<IResponse>(target));
        }

        public void Disable()
        {
            this.incomingBlockLink.Dispose();
            this.IsEnabled = false;
        }

        protected void Publish(IResponse response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            this.outgoingBlock.SendAsync(response);
        }

        protected virtual Task ProcessAsync(IRequest request)
        {
            return Task.CompletedTask;
        }
    }
}
