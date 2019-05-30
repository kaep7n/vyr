using System;
using System.Threading.Tasks;

namespace Vyr.Skills
{
    public interface ISkill
    {
        string Topic { get; }

        bool IsEnabled { get; }

        void Enable();

        Task EnqueueAsync(IRequest request);

        void Subscribe(Action<IResponse> target);

        void Disable();
    }
}
