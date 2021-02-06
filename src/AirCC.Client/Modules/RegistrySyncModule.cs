using AirCC.Client.Registry;
using BCI.Extensions.Core.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirCC.Client.Modules
{
    public class RegistrySyncModule : BackgroundService
    {
        private readonly ILogger<RegistrySyncModule> logger;
        private readonly AirCCConfigOptions airCCConfigOptions;
        private readonly IJsonSerializer jsonSerializer;

        public RegistrySyncModule(ILogger<RegistrySyncModule> logger, AirCCConfigOptions aicCCConfigOptions, 
            IJsonSerializer jsonSerializer)
        {
            this.logger = logger;
            this.airCCConfigOptions = aicCCConfigOptions;
            this.jsonSerializer = jsonSerializer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var app = new ApplicationRegistry { Id = airCCConfigOptions.ApplicationId,Url = airCCConfigOptions.PublicOrigin };
                    HttpClient httpClient = new HttpClient();
                    var url = $"{airCCConfigOptions.RegistryServiceUrl.TrimEnd('/')}/api/registry/register";
                    HttpContent httpContent = new StringContent(jsonSerializer.Serialize(app));
                    await httpClient.PostAsync(url, httpContent);
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
