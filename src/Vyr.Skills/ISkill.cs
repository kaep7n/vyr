using System;
using System.Threading.Tasks;

namespace Vyr.Skills
{
    public interface ISkill
    {
        bool IsEnabled { get; }

        void Enable();

        Task EnqueueAsync(Job job);

        void Disable();
    }
}
