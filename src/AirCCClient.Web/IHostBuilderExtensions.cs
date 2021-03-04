using AirCC.Client;
using AirCC.Client.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using AirCCClient.Web.Apis;
using BCI.Extensions.IdentityClient.Token;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AirCCClient.Web
{
    public static class IHostBuilderExtensions
    {
        public static IHostBuilder ConfigureAirCCFile(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services => {
                services.AddClientServices();
                services.TryAddScoped<IUpdateAuthorizationService, UpdateAuthorizationService>();
                services.TryAddTransient<IJwtTokenHandler,JwtTokenHandler>();
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
