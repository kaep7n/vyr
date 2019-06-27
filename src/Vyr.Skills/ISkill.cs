using System;
using System.Threading.Tasks;
using Vyr.Core;

namespace Vyr.Skills
{
    public interface ISkill
    {
        bool IsEnabled { get; }

        void Enable();

        Task EnqueueAsync(IMessage message);

        void Subscribe(Action<IMessage> message);

        void Disable();
    }
}
