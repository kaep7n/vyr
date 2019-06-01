using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vyr.Skills.Tests.Fakes
{
    public class DataflowSkillFake : DataflowSkill
    {
        private readonly List<IRequest> processedRequests = new List<IRequest>();

        public bool HasProcessedAnyRequests()
        {
            return this.processedRequests.Any();
        }

        public int GetProcessedRequestsCount()
        {
            return this.processedRequests.Count;
        }

        protected override Task ProcessAsync(IRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            this.processedRequests.Add(request);

            this.PublishAsync(new FakeResponse(request));

            return base.ProcessAsync(request);
        }
    }
}
