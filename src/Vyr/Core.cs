using System;
using System.Collections.Generic;
using Vyr.Isolation;

namespace Vyr
{
    public class Core
    {
        private readonly string[] assemblies;
        private readonly IIsolationStrategy isolationStrategy;
        private readonly List<IIsolation> isolations = new List<IIsolation>();

        public Core(IIsolationStrategy isolationStrategy, string[] assemblies)
        {
            if (isolationStrategy == null)
            {
                throw new ArgumentNullException(nameof(isolationStrategy));
            }

            this.isolationStrategy = isolationStrategy;
            this.assemblies = assemblies;
        }

        public void Start()
        {
            foreach (var assembly in this.assemblies)
            {
                var isolation = this.isolationStrategy.Create();
                isolation.Isolate(assembly);

                this.isolations.Add(isolation);
            }
        }

        public void Stop()
        {
            foreach (var isolation in this.isolations)
            {
                isolation.Free();
            }

            this.isolations.Clear();
        }
    }
}
