using AirCC.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace AirCCClient.Web
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder AddAirCCFile(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                var airccOptions = config.GetSection(AirCCConfigOptions.SectionName).Get<AirCCConfigOptions>();
                services.AddSingleton(airccOptions);
                if (!string.IsNullOrWhiteSpace(airccOptions.MainPath))
                {
                    if (!File.Exists(airccOptions.FilePath))
                        File.Create(airccOptions.FilePath);
                }
            });

            hostBuilder = hostBuilder.ConfigureAppConfiguration(configBuilder =>
            {
                var airccOptions = configBuilder.Build().GetSection(AirCCConfigOptions.SectionName).Get<AirCCConfigOptions>();
                configBuilder.AddIniFile(airccOptions.FilePath, optional: true, reloadOnChange: true);
            });
            return hostBuilder;
        }

    }
}
