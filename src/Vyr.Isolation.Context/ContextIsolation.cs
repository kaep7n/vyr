using System;
using System.Reflection;

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

        public void Free()
        {
            if (this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext.Unload();
            }
        }

        public object Isolate(AgentDescription agentDescription)
        {
            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(agentDescription.Assembly));
            var type = assembly.GetType(agentDescription.Type);

            return Activator.CreateInstance(type);
        }
    }
}
