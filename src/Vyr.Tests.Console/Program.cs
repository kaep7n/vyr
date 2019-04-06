using System;

namespace Vyr.Tests.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            System.Console.WriteLine($"Entering main method of assembly {assembly.FullName}");

            System.Console.WriteLine("doing some work");

            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine($"hey theres a number {i}");
            }

            System.Console.WriteLine("completed my work, thx");

            return 1;
        }
    }
}
