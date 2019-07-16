using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Vyr.Skills;

namespace Vyr.Isolation.Context
{
    public class ContextIsolation : IIsolation
    {
        private readonly string directory;
        private readonly WeakReference<DirectoryLoadContext> loadContextRef = new WeakReference<DirectoryLoadContext>(null, true);
        private object agent;

        public ContextIsolation(string directory)
        {
            this.directory = directory;
        }

        public Task IsolateAsync(IsolationConfiguration isolationConfiguration)
        {
            var serviceCollection = new ServiceCollection();

            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var agentAssembly = loadContext.LoadFromAssemblyName(new AssemblyName(isolationConfiguration.AgentConfiguration.Assembly));
            var agentType = agentAssembly.GetType(isolationConfiguration.AgentConfiguration.Type);
            serviceCollection.AddSingleton(agentType);

            foreach (var skillConfiguration in isolationConfiguration.AgentConfiguration.SkillConfigurations)
            {
                var skillAssembly = loadContext.LoadFromAssemblyName(new AssemblyName(skillConfiguration.Assembly));
                var skillType = agentAssembly.GetType(skillConfiguration.Type);
                serviceCollection.AddTransient(typeof(ISkill), skillType);
            }

            var provider = serviceCollection.BuildServiceProvider();
            this.agent = provider.GetRequiredService(agentType);

            return Task.CompletedTask;
        }

        public async Task RunAsync()
        {
            var runMethod = this.agent.GetType().GetMethod("RunAsync");
            await (Task)runMethod.Invoke(this.agent, new object[] { });
        }

        public async Task IdleAsync()
        {
            var runMethod = this.agent.GetType().GetMethod("IdleAsync");
            await (Task)runMethod.Invoke(this.agent, new object[] { });
        }

        public Task FreeAsync()
        {
            if (this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext.Unload();
            }

            return Task.CompletedTask;
        }
    }
}
