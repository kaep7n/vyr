using System;
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

            var main = type.GetMethod("Main");

            main.Invoke(program, new object[] { });
        }

        public void Down()
        {
            this.loadContext.Unload();
        }
    }
}
