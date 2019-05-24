using System;
using System.Net.Http;

namespace Homematic.Api.Xml
{
    public class HomematicApi
    {
        private readonly Uri baseAddress;
        private readonly IHttpClientFactory httpClientFactory;

        public HomematicApi(string baseAddress, IHttpClientFactory httpClientFactory)
        {
            if (baseAddress is null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (httpClientFactory is null)
            {
                throw new ArgumentNullException(nameof(httpClientFactory));
            }

            this.baseAddress = new Uri(baseAddress);
            this.httpClientFactory = httpClientFactory;
        }

        public GetDeviceListQuery GetDeviceListQuery()
        {
            var httpClient = this.httpClientFactory.CreateClient();
            httpClient.BaseAddress = this.baseAddress;

            return new GetDeviceListQuery(httpClient);
        }

        public GetDeviceStateQuery GetDeviceStateQuery(string deviceId)
        {
            var httpClient = this.httpClientFactory.CreateClient();
            httpClient.BaseAddress = this.baseAddress;

            return new GetDeviceStateQuery(httpClient, deviceId);
        }
    }
}
