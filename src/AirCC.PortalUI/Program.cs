using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AirCC.Portal.Exceptions;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using BCI.Extensions.Core.Json;
using BCI.Extensions.Newtonsoft;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using AirCC.PortalUI.HttpHandlers;

namespace AirCC.PortalUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //SetupLogger();
            //Log.Information("Hello, browser!");
            //https://blazorise.com/docs/usage/material/
            //https://blazorise.com/docs/components

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            //blazor material
            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped(sp => new HttpClient(
                sp.GetRequiredService<CustomAuthorizationMessageHandler>())
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            //var unhandledExceptionSender = new UnhandledExceptionSender();
            //var unhandledExceptionProvider = new UnhandledExceptionProvider(unhandledExceptionSender);
            //builder.Logging.AddProvider(unhandledExceptionProvider);
            //builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);

            builder.Services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();
            
            var host = builder.Build();
            host.Services
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();
            await host.RunAsync();
        }

        private static void SetupLogger()
        {
            //https://github.com/Scotty-Hudson/BlazorWasmLogUnhandledExceptions/blob/master/BlazorWasmLogUnhandledExceptions/Client/Program.cs
            var levelSwitch = new LoggingLevelSwitch();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
                //.Enrich.WithProperty(nameof(AppState.User), AppState.User)
                //.Enrich.WithProperty(nameof(AppState.UserCompany), AppState.UserCompany)
                .Enrich.WithExceptionDetails()
                .WriteTo.BrowserHttp(controlLevelSwitch: levelSwitch)
                .CreateLogger();
        }

    }
}
