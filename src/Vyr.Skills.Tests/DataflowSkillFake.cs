using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vyr.Skills.Tests
{
    public partial class DataflowSkillTests
    {
        public class DataflowSkillFake : DataflowSkill
        {
            private readonly List<Job> processedJobs = new List<Job>();

            public bool HasProcessedAnyJob()
            {
                return this.processedJobs.Any();
            }

            public int ProcessedJobsCount()
            {
                return this.processedJobs.Count;
            }

            protected override Task ProcessAsync(Job job)
            {
                this.processedJobs.Add(job);

                return base.ProcessAsync(job);
            }
        }
    }
}
