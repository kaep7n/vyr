using System;
using System.Collections.Generic;
using System.Text;

namespace Vyr.Core
{
    public class Test
    {
        public Test(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            this.Name = name;
        }

        public string Name { get; }
    }
}
