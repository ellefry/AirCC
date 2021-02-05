using AirCCClient.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCC.Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(configBuilder =>
                //{
                //    //configBuilder.AddJsonFile("dynamic.json", optional: true, reloadOnChange: true);
                //    //configBuilder.AddInMemoryCollection();
                //    configBuilder.AddIniFile(@"D:\ellefry\package.ini", true, true);
                //})
                .AddAirCCFile()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
