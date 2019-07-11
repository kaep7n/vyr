using System;

namespace Vyr.Core
{
    public interface IMessage
    {
        string Id { get; }

        string Topic { get; }

        DateTime CreatedAtUtc { get; }
    }
}
