using System;
using System.Collections.Generic;

namespace Vyr.Isolation
{
    public class AgentConfiguration
    {
        public AgentConfiguration(string assembly, string type, IEnumerable<SkillConfiguration> skillConfigurations)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (skillConfigurations is null)
            {
                throw new ArgumentNullException(nameof(skillConfigurations));
            }

            this.Assembly = assembly;
            this.Type = type;
            this.SkillConfigurations = skillConfigurations;
        }

        public string Assembly { get; }

        public string Type { get; }

        public IEnumerable<SkillConfiguration> SkillConfigurations { get; }
    }
}
