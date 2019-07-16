using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

        public ContextIsolation(string directory)
        {
            this.directory = directory;
        }

        public Task IsolateAsync(IsolationConfiguration isolationConfiguration)
        {
            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var controllerType = typeof(IsolationController);
            var controllerAssembly = loadContext.LoadFromAssemblyName(controllerType.Assembly.GetName());
            var controllerType1 = controllerAssembly.GetType(controllerType.FullName);

            var config = JsonConvert.SerializeObject(isolationConfiguration.AgentConfiguration);
            var contoller = Activator.CreateInstance(controllerType1,
                loadContext,
                config);

            return Task.CompletedTask;
        }

        //public async Task RunAsync()
        //{
        //    var runMethod = this.agent.GetType().GetMethod("RunAsync");
        //    await (Task)runMethod.Invoke(this.agent, new object[] { });
        //}

        //public async Task IdleAsync()
        //{
        //    var runMethod = this.agent.GetType().GetMethod("IdleAsync");
        //    await (Task)runMethod.Invoke(this.agent, new object[] { });
        //}

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
