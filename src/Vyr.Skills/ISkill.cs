using System.Collections.Generic;

namespace Vyr.Skills
{
    public interface ISkill
    {
        bool IsEnabled { get; }

        IEnumerable<string> Topics { get; }

        void Enable();

        void Disable();
    }
}
