using System;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;

namespace Vyr.Isolation.Context
{
    public class IsolationController : IIsolationController
    {
        private readonly IAgent agent;

        public IsolationController(AgentOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("vyr.agent.test.json")
            //    .Build();

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddLogging(c => c.AddConsole());
            //serviceCollection.AddSingleton<IConfiguration>(configuration);

            //var agent = options.Type;
            //var agentType = Type.GetType(agent);
            //serviceCollection.AddTransient(typeof(IAgent), agentType);

            //foreach (var skill in options.Skills)
            //{
            //    var skillType = Type.GetType(skill.Type);
            //    serviceCollection.AddTransient(typeof(ISkill), skillType);
            //}

            //var provider = serviceCollection.BuildServiceProvider();
            //this.agent = provider.GetRequiredService<IAgent>();
        }

        public async Task RunAsync()
        {
            await this.agent.RunAsync()
                .ConfigureAwait(false);
        }

        public async Task IdleAsync()
        {
            await this.agent.IdleAsync()
                .ConfigureAwait(false);
        }
    }
}
