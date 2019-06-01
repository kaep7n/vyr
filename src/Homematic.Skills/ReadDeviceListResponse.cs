using Homematic.Api.Xml;
using System;
using Vyr.Skills;

namespace Homematic.Skills
{
    public class ReadDeviceListResponse : IResponse
    {
        public ReadDeviceListResponse(Device device)
        {
            if (device is null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            this.Id = Guid.NewGuid().ToString();
            this.Device = device;
        }

        public string Id { get; }

        public Device Device { get; }
    }
}
