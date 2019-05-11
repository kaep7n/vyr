using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vyr.Agents;

namespace Vyr.Isolation.Context
{
    public class ContextIsolation : IIsolation
    {
        private readonly string directory;

        private readonly WeakReference<DirectoryLoadContext> loadContextRef = new WeakReference<DirectoryLoadContext>(null, true);

        private readonly List<Type> agentTypes = new List<Type>();

        public ContextIsolation(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.directory = directory;
        }

        public void Isolate(string assemblyName)
        {
            if (!this.loadContextRef.TryGetTarget(out var loadContext))
            {
                loadContext = new DirectoryLoadContext(this.directory);
                this.loadContextRef.SetTarget(loadContext);
            }

            var iAgentTypeName = typeof(IAgent).FullName;

            var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if(type.GetInterface(iAgentTypeName) != null)
                {
                    this.agentTypes.Add(type);
                }
            }
        }

        public void Free()
        {
            this.agentTypes.Clear();
        }
    }
}
