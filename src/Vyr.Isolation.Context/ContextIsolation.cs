using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Isolation.Context
{
    public class ContextIsolation : IIsolation
    {
        private readonly string directory;
        private readonly WeakReference<DirectoryLoadContext> loadContextRef = new WeakReference<DirectoryLoadContext>(null, true);
        private IIsolationController contoller;

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

            this.contoller = DispatchProxy.Create<IIsolationController, IsolationControllerProxy>();

            var controllerType = typeof(IsolationController);
            var controllerAssembly = loadContext.LoadFromAssemblyName(controllerType.Assembly.GetName());
            var controllerTypeFromLoadedAssembly = controllerAssembly.GetType(controllerType.FullName);

            var target = Activator.CreateInstance(controllerTypeFromLoadedAssembly, options);

            ((IsolationControllerProxy)this.contoller).SetTarget(target);

            await this.contoller.RunAsync()
                .ConfigureAwait(false);
        }

        public async Task FreeAsync()
        {
            await this.contoller.IdleAsync()
                .ConfigureAwait(false);


            if (this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext.Unload();
            }
        }
    }
}
