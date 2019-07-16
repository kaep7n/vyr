using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class Core
    {
        private readonly IIsolationStrategy isolationStrategy;
        private readonly IsolationConfiguration[] isolationConfigurations;

        private readonly List<IIsolation> isolations = new List<IIsolation>();

        public Core(IIsolationStrategy isolationStrategy, IsolationConfiguration[] isolationConfigurations)
        {
            if (isolationStrategy is null)
            {
                throw new ArgumentNullException(nameof(isolationStrategy));
            }

            if (isolationConfigurations is null)
            {
                throw new ArgumentNullException(nameof(isolationConfigurations));
            }

            this.isolationStrategy = isolationStrategy;
            this.isolationConfigurations = isolationConfigurations;
        }

        public async Task StartAsync()
        {
            foreach (var isolationConfiguration in this.isolationConfigurations)
            {
                var isolation = this.isolationStrategy.Create();
                this.isolations.Add(isolation);

                await isolation.IsolateAsync(isolationConfiguration);
            }
        }

        public async Task StopAsync()
        {
            foreach (var isolation in this.isolations)
            {
                await isolation.FreeAsync();
            }
        }
    }
}
