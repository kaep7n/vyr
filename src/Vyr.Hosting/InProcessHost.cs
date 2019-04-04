using System;
using System.Linq;
using System.Reflection;

namespace Vyr.Hosting
{
    public class InProcessHost : IHost
    {
        private InProcessLoadContext loadContext;

        public void Up()
        {
            this.loadContext = new InProcessLoadContext();
            var assembly = this.loadContext.LoadFromAssemblyName(new AssemblyName("Vyr"));

            var type = assembly.GetType("Vyr.Program");
            var program = Activator.CreateInstance(type);

            var parameters = new Type[] { typeof(string[]) };
            var main = type.GetRuntimeMethods().FirstOrDefault(m => m.Name == "Main");

            var args = new object[] { new string[] { } };

            var result = main.Invoke(program, args);
        }

        public void Down()
        {
            this.loadContext.Unload();
        }
    }
}
