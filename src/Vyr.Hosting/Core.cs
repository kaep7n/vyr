using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class Core
    {
        private readonly IIsolationStrategy isolationStrategy;
        private IIsolation isolation;

        public Core(IIsolationStrategy isolationStrategy)
        {
            if (isolationStrategy is null)
            {
                throw new ArgumentNullException(nameof(isolationStrategy));
            }

            this.isolationStrategy = isolationStrategy;
        }

        public async Task StartAsync()
        {
            this.isolation = this.isolationStrategy.Create();
         
            await this.isolation.IsolateAsync();
        }

        public async Task StopAsync()
        {
            await this.isolation.FreeAsync();
        }
    }
}
