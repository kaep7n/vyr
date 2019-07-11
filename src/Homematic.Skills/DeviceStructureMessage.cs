using Homematic.Api.Xml;
using System;
using Vyr.Core;

namespace Homematic.Skills
{
    public class DeviceStructureMessage : IMessage
    {
        public DeviceStructureMessage(Device device)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            this.Id = new Id();
            this.Topic = "DeviceInfo";
            this.Device = device;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAtUtc { get; }

        public Device Device { get; }
    }
}