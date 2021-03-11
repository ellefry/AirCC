using AirCC.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WatsonWebsocket;
using System.IdentityModel.Tokens.Jwt;

namespace NetCoreConsole.Demo
{
    class Program
    {
        static  void Main(string[] args)
        {
            CreateHost().Wait();

            Console.ReadKey();
        }

        private const string _appsettings = "appsettings.json";
        private const string _hostsettings = "hostsettings.json";

        private static async Task CreateHost()
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(_hostsettings, optional: true);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(_appsettings, optional: true);
                    configApp.AddJsonFile(
                        $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime()
                .ConfigureAirCcFile()
                .Build();

            await host.RunAsync();
        }
    }
}