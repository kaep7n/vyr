using System;

namespace Vyr.Skills.Tests
{
    public class JobResult
    {
        public JobResult(Job incomingJob)
        {
            if (incomingJob is null)
            {
                throw new ArgumentNullException(nameof(incomingJob));
            }

            this.IncomingJob = incomingJob;
        }

        public Job IncomingJob { get; }
    }
}
