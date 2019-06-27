using System;

namespace Vyr.Core
{
    public interface IMessage
    {
        string Id { get; }

        DateTime CreatedAt { get; }
    }
}
