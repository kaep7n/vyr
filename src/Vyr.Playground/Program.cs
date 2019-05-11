using System;
using System.IO;
using Vyr.Hosting;
using Vyr.Isolation.Context;

namespace Vyr.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var isolationStrategy = new ContextIsolationStrategy(Directory.GetCurrentDirectory());
            var assemblies = new [] { "Vyr.Playground.Agents" };

            Console.WriteLine("creating host");
            var host = new InProcessHost(isolationStrategy, assemblies);
            Console.WriteLine("created host");

            Console.WriteLine("starting host");
            host.Up();
            Console.WriteLine("started host");

            Console.WriteLine("stopping host");
            host.Down();
            Console.WriteLine("stopping host");
        }
    }
}
