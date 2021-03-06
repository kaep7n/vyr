﻿using Homematic.Api.Xml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vyr.Core;
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

        protected override async Task ProcessAsync(IMessage message)
        {
            if (!(message is ReadDeviceListMessage))
            {
                return;
            }

            var deviceList = await this.getDeviceListQuery.ExecuteAsync();

            foreach (var device in deviceList.Devices)
            {
                await this.PublishAsync(new DeviceStructureMessage(device));
            }
        }

        protected override IEnumerable<string> GetTopics()
        {
            return new[] { "ReadDeviceList" };
        }
    }
}
