using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;

namespace Vyr.Skills.Tests.Fakes
{
    public class DataflowSkillFake : DataflowSkill
    {
        private readonly List<IMessage> processedRequests = new List<IMessage>();

        public DataflowSkillFake(ISourceBlock<IMessage> source) 
            : base(source)
        {
        }

        public bool HasProcessedAnyRequests()
        {
            return this.processedRequests.Any();
        }

        public int GetProcessedRequestsCount()
        {
            return this.processedRequests.Count;
        }

        protected override async Task ProcessAsync(IMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.processedRequests.Add(message);

            await this.PublishAsync(new FakeResultMessage(message));

            await base.ProcessAsync(message);
        }
    }
}
