using System.Collections.Generic;

namespace Vyr.Core
{
    public class AgentOptions
    {
        public string Type { get; set; }

        public IEnumerable<SkillOptions> Skills { get; set; }
    }
}
