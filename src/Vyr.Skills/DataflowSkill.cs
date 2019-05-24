using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Vyr.Skills
{
    public class DataflowSkill : ISkill
    {
        private readonly ActionBlock<Job> incomingTargetBlock;
        private readonly BufferBlock<Job> incomingBlock = new BufferBlock<Job>();

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
            await this.incomingBlock.SendAsync(job);
        }

        public void Disable()
        {
            this.incomingBlock.Complete();

            this.IsEnabled = false;
        }

        protected virtual Task ProcessAsync(Job job)
        {
            return Task.CompletedTask;
        }
    }
}
