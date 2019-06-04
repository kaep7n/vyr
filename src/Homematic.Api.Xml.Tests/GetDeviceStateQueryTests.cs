using Homematic.Testing;
using Homematic.Testing.Properties;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Homematic.Api.Xml.Tests
{
    public class GetDeviceStateQueryTests
    {
        [Fact]
        public async Task ExecuteAsync()
        {
            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(Resources.DeviceState2074));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceStateQuery = new GetDeviceStateQuery(httpClient, "2074");

            var state = await getDeviceStateQuery.ExecuteAsync();

            Assert.NotNull(state);
        }
    }
}
