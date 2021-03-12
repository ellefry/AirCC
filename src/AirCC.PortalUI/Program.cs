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

namespace AirCC.PortalUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            SetupLogger();
            Log.Information("Hello, browser!");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            var unhandledExceptionSender = new UnhandledExceptionSender();
            var unhandledExceptionProvider = new UnhandledExceptionProvider(unhandledExceptionSender);
            builder.Logging.AddProvider(unhandledExceptionProvider);
            builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);

            await builder.Build().RunAsync();
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
