using Newtonsoft.Json;
using System;
using Vyr.Core;

namespace Vyr
{
    public class Program
    {
        static void Main(string[] args)
        {
            var f = JsonConvert.DeserializeObject("{}");

            var test = new Test("test");

            Console.WriteLine(test.Name);
        }
    }
}
