using System;
using System.Collections.Generic;
using System.Reflection;
using Vyr.Agents;

namespace Vyr.Isolation
{
    public class AgentInspector
    {
        private readonly string directory;
        private readonly string assemblyName;

        public AgentInspector(string directory, string assemblyName)
        {
            this.directory = directory;
            this.assemblyName = assemblyName;
        }

        public IEnumerable<Type> Inspect()
        {
            return null;
            //var loadContext = new DirectoryLoadContext(this.directory);

            //var iAgentTypeName = typeof(IAgent).FullName;

            //var assembly = loadContext.LoadFromAssemblyName(new AssemblyName(this.assemblyName));
            //var types = assembly.GetTypes();

            //foreach (var type in types)
            //{
            //    if (type.GetInterface(iAgentTypeName) != null)
            //    {
            //        yield return type;
            //    }
            //}
        }
    }
}
