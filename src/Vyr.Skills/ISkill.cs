using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vyr.Skills
{
    public interface ISkill
    {
        bool IsEnabled { get; }

        IEnumerable<string> Topics { get; }

        Task EnableAsync();

        Task DisableAsync();
    }
}
