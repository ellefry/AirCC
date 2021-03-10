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
        private readonly AirCCConfigOptions airCCConfigOptions;
        private readonly IJsonSerializer jsonSerializer;
        private readonly AirCcWsClient airCcWsClient;

        public RegistrySyncModule(ILogger<RegistrySyncModule> logger, AirCCConfigOptions aicCCConfigOptions, 
            IJsonSerializer jsonSerializer, AirCcWsClient airCcWsClient)
        {
            this.logger = logger;
            this.airCCConfigOptions = aicCCConfigOptions;
            this.jsonSerializer = jsonSerializer;
            this.airCcWsClient = airCcWsClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        //var app = new ApplicationRegistry { Id = airCCConfigOptions.ApplicationId,Url = airCCConfigOptions.PublicOrigin };
            //        //HttpClient httpClient = new HttpClient();
            //        //var url = $"{airCCConfigOptions.RegistryServiceUrl.TrimEnd('/')}/api/registry/register";
            //        //HttpContent httpContent = new StringContent(jsonSerializer.Serialize(app));
            //        //httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            //        //var response = await httpClient.PostAsync(url, httpContent);
            //        //if (response.StatusCode != HttpStatusCode.OK)
            //        //{
            //        //    logger.LogError($"[Registry error:] {await response.Content.ReadAsStringAsync()}");
            //        //}
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex.Message + Environment.NewLine + ex.StackTrace);
            //    }
            //    await Task.Delay(10000, stoppingToken);//等待10秒
            //}
            airCcWsClient.Initialize();
        }
    }
}
