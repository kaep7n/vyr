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
            var result = await this.httpClient.GetAsync("devicelist.cgi");

            using var resultStream = await result.Content.ReadAsStreamAsync();

            var serializer = new XmlSerializer(typeof(DeviceList));
            return (DeviceList)serializer.Deserialize(resultStream);
        }
    }
}
