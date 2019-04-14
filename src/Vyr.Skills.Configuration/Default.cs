using Microsoft.Extensions.Configuration;

namespace Vyr.Skills.Configuration
{
    public class Default : IConfigurationSkill
    {
        private readonly IConfiguration configuration;

        public Default()
        {
            //var workingDirectory = Directory.GetCurrentDirectory();

            var configurationBuilder = new ConfigurationBuilder()
                //.SetBasePath(workingDirectory)
                .AddJsonFile("vyr.settings.json", optional: false, reloadOnChange: true);

            this.configuration = configurationBuilder.Build();
        }

        public string Give(string key)
        {
            return this.configuration[key];            
        }
    }
}
