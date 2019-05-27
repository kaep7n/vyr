using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {
        private readonly ITargetBlock<Job> incomingTargetBlock;
        private readonly BufferBlock<Job> incomingBlock = new BufferBlock<Job>();

        private readonly BroadcastBlock<object> outgoingBlock = new BroadcastBlock<object>(i => { return i; });

        public DataflowSkill()
        {
            this.incomingTargetBlock = new ActionBlock<Job>(this.ProcessAsync);
        }

        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            this.incomingBlock.LinkTo(this.incomingTargetBlock, new DataflowLinkOptions { PropagateCompletion = true });
            this.IsEnabled = true;
        }

        public async Task EnqueueAsync(Job job)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            await this.incomingBlock.SendAsync(job);
        }

        public void Subscribe(Action<object> target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            this.outgoingBlock.LinkTo(new ActionBlock<object>(target));
        }

        public void Disable()
        {
            this.IsEnabled = false;
        }

        protected void Publish(object result)
        {
            this.outgoingBlock.SendAsync(result);
        }

        protected virtual Task ProcessAsync(Job job)
        {
            return Task.CompletedTask;
        }
    }
}
