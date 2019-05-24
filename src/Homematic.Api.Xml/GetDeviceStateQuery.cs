using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Homematic.Api.Xml
{
    public class GetDeviceStateQuery
    {
        private readonly HttpClient httpClient;
        private readonly string deviceId;

        public GetDeviceStateQuery(HttpClient httpClient, string deviceId)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (deviceId is null)
            {
                throw new ArgumentNullException(nameof(deviceId));
            }

            this.httpClient = httpClient;
            this.deviceId = deviceId;
        }

        public async Task<State> ExecuteAsync()
        {
            var result = await this.httpClient.GetAsync($"state.cgi?device_id={this.deviceId}");

            using var resultStream = await result.Content.ReadAsStreamAsync();

            var serializer = new XmlSerializer(typeof(State));
            return (State)serializer.Deserialize(resultStream);
        }
    }
}
