using System;

namespace Vyr.Isolation
{
    public class SkillConfiguration
    {
        public SkillConfiguration(string assembly, string type)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Assembly = assembly;
            this.Type = type;
        }

        public string Assembly { get; }

        public string Type { get; }
    }
}
