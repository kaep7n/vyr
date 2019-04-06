using System;

namespace Vyr.Tests.Library
{
    public class Process
    {
        public int Run()
        {
            var assembly = typeof(Process).Assembly;
            Console.WriteLine($"Entering run method of assembly {assembly.FullName}");

            Console.WriteLine("doing some work");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"hey theres a number {i}");
            }

            Console.WriteLine("completed my work, thx");

            return 2;
        }
    }
}
