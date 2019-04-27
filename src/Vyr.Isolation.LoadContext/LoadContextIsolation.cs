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

        public object CreateInstance(string typeName)
        {
            return this.assembly.CreateInstance(typeName, true);
        }

        public void Isolate(string assemblyName)
        {
            this.assembly = this.loadContext.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
    }
}
