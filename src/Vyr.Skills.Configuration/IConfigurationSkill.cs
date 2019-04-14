using System;

namespace Vyr.Skills.Configuration
{
    public interface IConfigurationSkill : ISkill
    {
        string Give(string key);
    }
}
