using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Vyr.Isolation.Context
{
    public class ContextIsolation : IIsolation
    {
        private readonly string directory;
        private readonly WeakReference<DirectoryLoadContext> loadContextRef = new WeakReference<DirectoryLoadContext>(null, true);
        private object contoller;

        public ContextIsolation(string directory)
        {
            this.directory = directory;
        }

        public async Task IsolateAsync()
        {
            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var controllerType = typeof(IsolationController);
            var controllerAssembly = loadContext.LoadFromAssemblyName(controllerType.Assembly.GetName());
            var controllerTypeFromLoadedAssembly = controllerAssembly.GetType(controllerType.FullName);

            this.contoller = Activator.CreateInstance(controllerTypeFromLoadedAssembly);

            await (Task)this.contoller
                .GetType()
                .GetMethod("RunAsync")
                .Invoke(this.contoller, null);
        }

        public async Task FreeAsync()
        {
            await (Task)this.contoller
               .GetType()
               .GetMethod("IdleAsync")
               .Invoke(this.contoller, null);

            if (this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext.Unload();
            }
        }
    }
}
