using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Vyr.Core;

namespace Vyr.Skills
{
    public interface ISkill
    {
        bool IsEnabled { get; }

        string[] AcceptedTopics { get; }

        void Enable();

        void Subscribe(Action<IMessage> message);

        void Disable();
    }
}
