using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;
using Vyr.Skills;

namespace Vyr.Isolation.Context
{
    public class IsolationController
    {
        private readonly IAgent agent;

        public IsolationController(string options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var agentOptions = JsonConvert.DeserializeObject<AgentOptions>(options);
            var serviceCollection = new ServiceCollection();

            var agent = agentOptions.Type;
            var agentType = Type.GetType(agent);
            serviceCollection.AddTransient(typeof(IAgent), agentType);

            foreach (var skill in agentOptions.Skills)
            {
                var skillType = Type.GetType(skill.Type);
                serviceCollection.AddTransient(typeof(ISkill), skillType);
            }

            var provider = serviceCollection.BuildServiceProvider();
            this.agent = provider.GetRequiredService<IAgent>();
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
