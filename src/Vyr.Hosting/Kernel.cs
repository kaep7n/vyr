using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Vyr.Core;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class Kernel : IHostedService
    {
        private readonly KernelOptions options;
        private readonly IIsolationStrategy isolationStrategy;
        private readonly ILogger<Kernel> logger;
        private readonly IList<IIsolation> isolations = new List<IIsolation>();

        public Kernel(IIsolationStrategy isolationStrategy,IOptions<KernelOptions> options, ILogger<Kernel> logger)
        {
            if (isolationStrategy is null)
            {
                throw new ArgumentNullException(nameof(isolationStrategy));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.options = options.Value;
            this.isolationStrategy = isolationStrategy;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("starting kernel");

            foreach (var agent in this.options.Agents)
            {
                var isolation = this.isolationStrategy.Create();

                await isolation.IsolateAsync(agent)
                    .ConfigureAwait(false);

                this.isolations.Add(isolation);
            }

            this.WriteContexts();

            this.logger.LogInformation("started kernel");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("stopping kernel");

            foreach (var isolation in this.isolations)
            {
                await isolation.FreeAsync()
                    .ConfigureAwait(false);
            }

            this.WriteContexts();

            this.logger.LogInformation("stopped kernel");
        }

        private void WriteContexts()
        {
            foreach (var context in AssemblyLoadContext.All)
            {
                this.logger.LogTrace($"context: {context.Name}");

                foreach (var assembly in context.Assemblies)
                {
                    this.logger.LogDebug("assembly: " + assembly.FullName);
                }
            }
        }
    }
}
