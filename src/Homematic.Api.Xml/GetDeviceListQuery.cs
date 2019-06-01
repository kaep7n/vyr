using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    public class GetDeviceListQuery
    {
        private readonly HttpClient httpClient;

        public GetDeviceListQuery(HttpClient httpClient)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            this.httpClient = httpClient;
        }

        public async Task<DeviceList> ExecuteAsync()
        {
            using var stream = await this.httpClient.GetStreamAsync("devicelist.cgi");

            var serializer = new XmlSerializer(typeof(DeviceList));
            return (DeviceList)serializer.Deserialize(stream);
        }
    }
}
