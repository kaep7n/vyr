using System;
using System.Reflection;

namespace Vyr.Isolation.LoadContext
{
    public class LoadContextIsolation : IIsolation
    {
        private Assembly assembly = null;
        private readonly DirectoryLoadContext loadContext;

        public LoadContextIsolation(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.loadContext = new DirectoryLoadContext(directory);
        }

        public object Create(string typeName)
        {
            var type = this.assembly.GetType(typeName);
            return Activator.CreateInstance(type);
        }

        public void Isolate(string assemblyName)
        {
            this.assembly = this.loadContext.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
    }
}
