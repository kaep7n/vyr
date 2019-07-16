using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Vyr.Hosting;
using Vyr.Isolation;
using Vyr.Isolation.Context;

namespace Vyr.Playground
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var isolationStrategy = new ContextIsolationStrategy(Directory.GetCurrentDirectory());
            var isolationConfigurations = new IsolationConfiguration[]
            {
                new IsolationConfiguration(
                    new AgentConfiguration(
                        "Vyr.Playground.Agents",
                        "Vyr.Playground.Agents.CoreAgent",
                         new SkillConfiguration[] {
                             new SkillConfiguration(
                                 "Vyr.Playground.Agents",
                                 "Vyr.Playground.Agents.TestSkill1"),
                             new SkillConfiguration(
                                 "Vyr.Playground.Agents",
                                 "Vyr.Playground.Agents.TestSkill2")
                            })
                         )
            };

            WriteContexts();

            Console.WriteLine("creating host");
            var host = new InProcessHost(isolationStrategy, isolationConfigurations);
            Console.WriteLine("created host");

            Console.WriteLine("starting host");
            await host.UpAsync();
            Console.WriteLine("started host");

            WriteContexts();

            Console.WriteLine("stopping host");
            await host.DownAsync();
            Console.WriteLine("stopping host");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            WriteContexts();
        }

        private static void WriteContexts()
        {
            foreach (var context in AssemblyLoadContext.All)
            {
                Console.WriteLine("Context: " + context.Name);

                foreach (var assembly in context.Assemblies)
                {
                    Console.WriteLine("          Assembly: " + assembly.FullName);
                }
            }
        }
    }
}
