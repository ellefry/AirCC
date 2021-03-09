using AirCC.Client;
using AirCC.Client.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
//using BCI.Extensions.IdentityClient.Token;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AirCC.Client
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureAirCCFile(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => {
                services.AddClientServices();
            });

            hostBuilder = hostBuilder.ConfigureAppConfiguration(configBuilder =>
            {
                var airCcConfigOptions = configBuilder.Build().GetSection(AirCCConfigOptions.SectionName).Get<AirCCConfigOptions>();
                configBuilder.AddIniFile(airCcConfigOptions.FilePath, optional: true, reloadOnChange: true);
            });
            return hostBuilder;
        }

    }
}
