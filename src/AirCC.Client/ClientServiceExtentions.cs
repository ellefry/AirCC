using AirCC.Client.Modules;
using BCI.Extensions.Core.Json;
using BCI.Extensions.Newtonsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AirCC.Client
{
    public static class ClientServiceExtentions
    {
        public static void AddClientServices(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var airccOptions = config.GetSection(AirCCConfigOptions.SectionName).Get<AirCCConfigOptions>();
            if (airccOptions == null)
                throw new ApplicationException($"No {AirCCConfigOptions.SectionName} configuration");

            services.TryAddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            services.AddScoped<IAirCCSettingsService, AirCCSettingsService>();
            services.AddHostedService<RegistrySyncModule>();
            if (!string.IsNullOrWhiteSpace(airccOptions.MainPath))
            {
                if (!File.Exists(airccOptions.FilePath))
                    File.Create(airccOptions.FilePath);
            }
        }
    }
}
