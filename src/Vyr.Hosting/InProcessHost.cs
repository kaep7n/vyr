using System.Threading.Tasks;
using Vyr.Isolation;

namespace Vyr.Hosting
{
    public class InProcessHost : IHost
    {
        private readonly Core core;

        public InProcessHost(IIsolationStrategy isolationStrategy, IsolationConfiguration[] isolationConfigurations)
        {
            this.core = new Core(isolationStrategy, isolationConfigurations);
        }

        public async Task UpAsync()
        {
            await this.core.StartAsync();
        }

        public async Task DownAsync()
        {
            await this.core.StopAsync();
        }
    }
}
