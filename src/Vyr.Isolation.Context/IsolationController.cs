using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Vyr.Agents;
using Vyr.Skills;

namespace Vyr.Isolation.Context
{
    public class IsolationController
    {
        private readonly IAgent agent;

        public IsolationController()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("vyr.agent.json")
                .Build();

            var serviceCollection = new ServiceCollection();

            var agent = configuration["agent"];
            var agentType = Type.GetType(agent);
            serviceCollection.AddTransient(typeof(IAgent), agentType);

            foreach (var skillConfiguration in configuration.GetSection("skills").GetChildren())
            {
                var skill = skillConfiguration.Value;
                var skillType = Type.GetType(skill);
                serviceCollection.AddTransient(typeof(ISkill), skillType);
            }

            var provider = serviceCollection.BuildServiceProvider();
            this.agent = provider.GetRequiredService<IAgent>();
        }

        public async Task RunAsync()
        {
            await this.agent.RunAsync();
        }

        public async Task IdleAsync()
        {
            await this.agent.IdleAsync();
        }
    }
}
