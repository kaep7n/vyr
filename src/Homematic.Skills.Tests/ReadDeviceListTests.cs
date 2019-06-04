using Homematic.Api.Xml;
using Homematic.Testing;
using Homematic.Testing.Properties;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Vyr.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Homematic.Skills.Tests
{
    public class ReadDeviceListTests
    {
        private readonly ITestOutputHelper output;

        public ReadDeviceListTests(ITestOutputHelper output)
        {
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            this.output = output;
        }

        [Fact]
        public async Task Test()
        {
            var stopwatch = Stopwatch.StartNew();

            var responseCount = 0;

            var httpClient = new HttpClient(new FakeHomematicHttpMessageHandler(Resources.Devicelist));
            httpClient.BaseAddress = new Uri("http://baseurimustbeset.com/");

            var getDeviceListQuery = new GetDeviceListQuery(httpClient);

            var skill = new ReadDeviceList(getDeviceListQuery);
            skill.Enable();

            skill.Subscribe(r =>
            {
                responseCount++;
                this.output.WriteLine($"{stopwatch.Elapsed} {responseCount} {((ReadDeviceListResponse)r).Device.Name}");
            });

            this.output.WriteLine($"{stopwatch.Elapsed} sending request");
            await skill.EnqueueAsync(new ReadDeviceListRequest());
            this.output.WriteLine($"{stopwatch.Elapsed} sent request");

            Thread.Sleep(2000);

            Assert.Equal(25, responseCount);
        }
    }
}
