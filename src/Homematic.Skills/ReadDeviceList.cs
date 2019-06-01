using Homematic.Api.Xml;
using System;
using System.Threading.Tasks;
using Vyr.Skills;

namespace Homematic.Skills
{
    public class ReadDeviceList : DataflowSkill
    {
        private readonly GetDeviceListQuery getDeviceListQuery;

        public ReadDeviceList(GetDeviceListQuery getDeviceListQuery)
        {
            if (getDeviceListQuery is null)
            {
                throw new ArgumentNullException(nameof(getDeviceListQuery));
            }

            this.getDeviceListQuery = getDeviceListQuery;
        }

        protected override async Task ProcessAsync(IRequest request)
        {
            if (!(request is ReadDeviceListRequest))
            {
                return;
            }

            var deviceList = await this.getDeviceListQuery.ExecuteAsync().ConfigureAwait(false);

            foreach (var device in deviceList.Devices)
            {
                this.Publish(new ReadDeviceListResponse(device));
            }
        }
    }
}
