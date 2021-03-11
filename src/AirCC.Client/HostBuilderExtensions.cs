using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AirCC.Client
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureAirCcFile(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
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
