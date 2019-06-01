using System;
using System.Collections.Generic;
using System.Text;
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
