
using System;

namespace Vyr.Isolation
{
    public interface IIsolation
    {
        object Isolate(AgentDescription agentDescription);

        void Free();
    }
}
