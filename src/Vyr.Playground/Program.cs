using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using Vyr.Core;
using Vyr.Hosting;
using Vyr.Isolation;
using Vyr.Isolation.Context;

namespace Vyr.Playground
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync()
                .ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(c =>
            {
                c.AddJsonFile("vyr.core.json", false);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IIsolationStrategy>(p => new ContextIsolationStrategy(Directory.GetCurrentDirectory()));

                services.Configure<KernelOptions>(hostContext.Configuration.GetSection("kernel"));
                services.AddHostedService<Kernel>();
            });
    }
}
