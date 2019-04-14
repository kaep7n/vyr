using System;

namespace Vyr.Isolation
{
    public interface IIsolation
    {
        void Isolate(string assemblyName);
    }
}
