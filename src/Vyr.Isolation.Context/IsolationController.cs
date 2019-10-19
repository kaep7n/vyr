using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Core;
using Vyr.Skills;

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

            var serviceCollection = new ServiceCollection();

            var agent = options.Type;
            var agentType = Type.GetType(agent);
            serviceCollection.AddTransient(typeof(IAgent), agentType);

            foreach (var skill in options.Skills)
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
