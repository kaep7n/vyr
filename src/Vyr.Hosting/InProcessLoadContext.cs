using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Vyr.Hosting
{
    public class InProcessLoadContext : AssemblyLoadContext
    {
        public InProcessLoadContext()
            : base(true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name.StartsWith("Vyr"))
            {
                return this.LoadFromAssemblyPath($@"C:\Users\kaept\source\repos\kaep7n\vyr\src\Vyr\bin\Debug\netcoreapp3.0\{assemblyName.Name}.dll");
            }
            else
            {
                return null;
            }
        }
    }
}
