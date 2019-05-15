using System;
using System.IO;
using System.Runtime.Loader;
using Vyr.Hosting;
using Vyr.Isolation;
using Vyr.Isolation.Context;

namespace Vyr.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var isolationStrategy = new ContextIsolationStrategy(Directory.GetCurrentDirectory());
            var agents = new AgentDescription[]
            {
                new AgentDescription("Vyr.Playground.Agents", "Vyr.Playground.Agents.Mulder"),
                new AgentDescription("Vyr.Playground.Agents", "Vyr.Playground.Agents.Skully")
            };

            WriteContexts();

            Console.WriteLine("creating host");
            var host = new InProcessHost(isolationStrategy, agents);
            Console.WriteLine("created host");

            Console.WriteLine("starting host");
            host.Up();
            Console.WriteLine("started host");

            WriteContexts();

            Console.WriteLine("stopping host");
            host.Down();
            Console.WriteLine("stopping host");

            GC.Collect();
            GC.WaitForFullGCComplete();

            WriteContexts();
        }

        private static void WriteContexts()
        {
            foreach (var context in AssemblyLoadContext.All)
            {
                Console.WriteLine("Context: " + context.Name);
            }
        }
    }
}
