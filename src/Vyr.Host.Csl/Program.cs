using System;
using Vyr.Isolation.LoadContext;

namespace Vyr.Host.Csl
{
    class Program
    {
        static void Main(string[] args)
        {
            var configIsolation = new LoadContextIsolation( @"C:\Users\kaept\source\repos\kaep7n\vyr\src\Vyr.Skills.Configuration\bin\Debug\netcoreapp3.0");
            configIsolation.Isolate("Vyr.Skills.Configuration");
            var defaultConfiguration = configIsolation.Create("Vyr.Skills.Configuration.Default");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Console.WriteLine("creating core");
            var core = new Core();
            Console.WriteLine("created core");

            Console.WriteLine("starting core");
            core.Start();
            Console.WriteLine("started core");

            Console.WriteLine("stopping core");
            core.Stop();
            Console.WriteLine("stopping core");
        }
    }
}
