using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Loader;
using Vyr.Agents;
using Vyr.Skills;

namespace Vyr.Isolation.Context
{
    public class IsolationController
    {
        private readonly AssemblyLoadContext context;
        private readonly AgentConfiguration agentConfiguration;
        private readonly IAgent agent;

        public IsolationController(AssemblyLoadContext context, AgentConfiguration agentConfiguration)
        {
            this.context = context;
            this.agentConfiguration = agentConfiguration;

            var serviceCollection = new ServiceCollection();

            var agentAssembly = this.context.LoadFromAssemblyName(new AssemblyName(this.agentConfiguration.Assembly));
            var agentType = agentAssembly.GetType(this.agentConfiguration.Type);

            serviceCollection.AddTransient(typeof(IAgent), agentType);

            foreach (var skillConfiguration in this.agentConfiguration.SkillConfigurations)
            {
                var skillAssembly = this.context.LoadFromAssemblyName(new AssemblyName(skillConfiguration.Assembly));
                var skillType = agentAssembly.GetType(skillConfiguration.Type);
                serviceCollection.AddTransient(typeof(ISkill), skillType);
            }

            var provider = serviceCollection.BuildServiceProvider();
            this.agent = provider.GetRequiredService<IAgent>();
        }
    }
}
