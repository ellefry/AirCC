using AirCC.Client.Registry;
using BCI.Extensions.Core.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirCC.Client.Modules
{
    public class RegistrySyncModule : BackgroundService
    {
        private readonly ILogger<RegistrySyncModule> logger;
        private readonly AirCCConfigOptions airCcConfigOptions;
        private readonly AirCcWsClient airCcWsClient;

        public RegistrySyncModule(ILogger<RegistrySyncModule> logger, AirCCConfigOptions airCcConfigOptions, 
            AirCcWsClient airCcWsClient)
        {
            this.logger = logger;
            this.airCcConfigOptions = airCcConfigOptions;
            this.airCcWsClient = airCcWsClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //airCcWsClient.Start();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!airCcWsClient.Connected)
                    {
                        //airCcWsClient.Stop();
                        airCcWsClient.Start();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
                }
                await Task.Delay(10000, stoppingToken);//等待10秒
            }
            
        }
    }
}
