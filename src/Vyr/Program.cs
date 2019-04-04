using System;
using Vyr.Core;

namespace Vyr
{
    public class Program
    {
        static void Main(string[] args)
        {
            var test = new Test("test");

            Console.WriteLine(test.Name);
        }
    }
}
