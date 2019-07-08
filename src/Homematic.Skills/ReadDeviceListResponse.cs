using Homematic.Api.Xml;
using System;
using Vyr.Core;

namespace Homematic.Skills
{
    public class ReadDeviceListResponse : IMessage
    {
        public ReadDeviceListResponse(Device device)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            this.Id = new Id();
            this.Device = device;
        }

        public string Id { get; }

        public string Topic { get; }

        public DateTime CreatedAt { get; }

        public Device Device { get; }
    }
}