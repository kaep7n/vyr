using System;
using Vyr.Skills;

namespace Homematic.Skills
{
    public class ReadDeviceListRequest : IRequest
    {
        public ReadDeviceListRequest()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
    }
}
