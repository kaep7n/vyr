using Newtonsoft.Json;
using System;
using System.Linq;
using Vyr.Hosting;

namespace Vyr.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonConvert.DeserializeObject("{}");

            var host = new InProcessHost();
            host.Up();

            WriteAllAssemblies();

            Console.WriteLine("Host is up");

            host.Down();
            GC.Collect();

            WriteAllAssemblies();

            Console.WriteLine("Host is down");

            Console.ReadLine();
        }

        private static void WriteAllAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies.OrderBy(a => a.FullName))
            {
                Console.WriteLine(assembly.FullName);
            }
        }

    }
}
