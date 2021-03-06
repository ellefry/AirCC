﻿using AirCC.Client.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace AirCC.Client
{
    public static class ClientServiceExtensions
    {
        public static void AddClientServices(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var airCcConfigOptions = config.GetSection(AirCCConfigOptions.SectionName).Get<AirCCConfigOptions>();
            if (airCcConfigOptions == null)
                throw new ApplicationException($"No {AirCCConfigOptions.SectionName} configuration");
            services.AddSingleton(airCcConfigOptions);
            services.TryAddSingleton<IAirCCSettingsService, AirCCSettingsService>();
            services.AddSingleton<AirCcWsClient>();
            services.AddHostedService<RegistrySyncModule>();
            if (!string.IsNullOrWhiteSpace(airCcConfigOptions.MainPath))
            {
                if (!Directory.Exists(airCcConfigOptions.MainPath))
                    Directory.CreateDirectory(airCcConfigOptions.MainPath);
                if (!File.Exists(airCcConfigOptions.FilePath))
                    File.Create(airCcConfigOptions.FilePath);
            }
        }
    }
}
