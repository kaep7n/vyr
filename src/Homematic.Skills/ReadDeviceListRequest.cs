using System;
using Vyr.Core;

namespace Homematic.Skills
{
    public class ReadDeviceListRequest : IMessage
    {
        public ReadDeviceListRequest()
        {
            this.Id = new Id();
            this.CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; }

        public DateTime CreatedAt { get; }
    }
}
