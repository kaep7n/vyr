using System;
using System.IO;
using System.Linq;
using Vyr.Hosting;

namespace Vyr.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var workingConsoleDirectory = @"C:\Users\kaept\Source\Repos\kaep7n\vyr\src\Vyr.Tests.Console\bin\Debug\netcoreapp3.0";
            var workingLibraryDirectory = @"C:\Users\kaept\Source\Repos\kaep7n\vyr\src\Vyr.Tests.Library\bin\Debug\netcoreapp3.0";

            var consoleHost = new InProcessHost(workingConsoleDirectory, "Vyr.Tests.Console");
            var libraryHost = new InProcessHost(workingLibraryDirectory, "Vyr.Tests.Library");

            string input;

            do
            {
                try
                {
                    consoleHost.Up();
                    Console.WriteLine("Console Host is up");
                    consoleHost.Down();
                    Console.WriteLine("Console Host is down");

                    //libraryHost.Up();
                    //Console.WriteLine("Library Host is up");
                    //libraryHost.Down();
                    //Console.WriteLine("Library Host is down");

                    WriteAllAssemblies();
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.Message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                finally
                {
                    input = Console.ReadLine();
                }
            }
            while (input != "exit");
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
