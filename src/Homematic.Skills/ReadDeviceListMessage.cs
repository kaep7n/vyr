using System;
using Vyr.Core;

namespace Homematic.Skills
{
    public class ReadDeviceListMessage : IMessage
    {
        public ReadDeviceListMessage()
        {
            this.Id = new Id();
            this.Topic = "ReadDeviceList";
            this.CreatedAtUtc = DateTime.UtcNow;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAtUtc { get; }
    }
}
