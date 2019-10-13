using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Isolation.Context
{
    public class ContextIsolation : IIsolation
    {
        private readonly string directory;
        private readonly WeakReference<DirectoryLoadContext> loadContextRef = new WeakReference<DirectoryLoadContext>(null, true);
        private object contoller;

        public ContextIsolation(string directory)
        {
            if (directory is null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.directory = directory;
        }

        public async Task IsolateAsync(AgentOptions options)
        {
            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var controllerType = typeof(IsolationController);
            var controllerAssembly = loadContext.LoadFromAssemblyName(controllerType.Assembly.GetName());
            var controllerTypeFromLoadedAssembly = controllerAssembly.GetType(controllerType.FullName);
            var serializedOptions = JsonConvert.SerializeObject(options);

            this.contoller = Activator.CreateInstance(controllerTypeFromLoadedAssembly, serializedOptions);

            await ((Task)this.contoller
                .GetType()
                .GetMethod("RunAsync")
                .Invoke(this.contoller, null))
                .ConfigureAwait(false);
        }

        public async Task FreeAsync()
        {
            await ((Task)this.contoller
               .GetType()
               .GetMethod("IdleAsync")
               .Invoke(this.contoller, null))
               .ConfigureAwait(false);

            if (this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext.Unload();
            }
        }
    }
}
