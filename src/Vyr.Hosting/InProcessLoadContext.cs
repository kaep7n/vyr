using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Vyr.Hosting
{
    public class InProcessLoadContext : AssemblyLoadContext
    {
        private readonly string workingDirectory;

        public InProcessLoadContext(string workingDirectory)
            : base(true)
        {
            this.workingDirectory = workingDirectory;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assemblyPath = Path.Combine(this.workingDirectory, $"{assemblyName.Name}.dll");

            if (File.Exists(assemblyPath))
            {
                return this.LoadFromAssemblyPath(assemblyPath);
            }
            else
            {
                return null;
            }
        }
    }
}
