using System;

namespace Vyr.Skills
{
    public class Job
    {
        public Job(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        public string Id { get; }
    }
}
