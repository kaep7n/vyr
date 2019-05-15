using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Vyr.Isolation.Context
{
    public class DirectoryLoadContext : AssemblyLoadContext
    {
        private readonly string directory;

        public DirectoryLoadContext(string directory)
            : base("DirectoryLoadContext", true)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.directory = directory;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assemblyPath = Path.Combine(this.directory, $"{assemblyName.Name}.dll");

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
