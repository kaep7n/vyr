using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vyr.Hosting
{
    public class InProcessHost : IHost
    {
        private readonly string workingDirectory;
        private readonly string name;

        private WeakReference loadContextReference = null;

        public InProcessHost(string workingDirectory, string name)
        {
            this.workingDirectory = workingDirectory;
            this.name = name;
        }

        public bool IsAlive => this.loadContextReference.IsAlive;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Up()
        {
            var inProcessLoadContext = new InProcessLoadContext(this.workingDirectory);
            this.loadContextReference = new WeakReference(inProcessLoadContext, true);

            var assembly = inProcessLoadContext.LoadFromAssemblyName(new AssemblyName(this.name));

            if (assembly.EntryPoint != null)
            {
                var args = new object[1] { new string[] { } };
                assembly.EntryPoint.Invoke(null, args);
            }
            else
            {
                var type = assembly.GetTypes().First(t => t.Name == "Process");
                var instance = Activator.CreateInstance(type);

                type.GetMethod("Run").Invoke(instance, null);
            }
        }

        public void Down()
        {
            var inProcessLoadContext = this.loadContextReference.Target as InProcessLoadContext;

            inProcessLoadContext.Unload();
        }
    }
}
