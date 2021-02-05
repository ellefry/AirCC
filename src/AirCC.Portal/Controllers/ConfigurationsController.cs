using AirCC.Client;
using AirCC.Portal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IOptions<AirCCModel> portalOptions;
        private readonly IOptionsMonitor<AirCCModel> portalOptionsMonitor;

        public ConfigurationsController(IConfiguration configuration, IOptions<AirCCModel> portalOptions, 
            IOptionsMonitor<AirCCModel> portalOptionsMonitor, AirCCConfigOptions airCCConfigOptions)
        {
            this.configuration = configuration;
            this.portalOptions = portalOptions;
            this.portalOptionsMonitor = portalOptionsMonitor;
        }

        [HttpGet]

        public IActionResult Get()
        {
            return Ok(new
            {
                Number = configuration["dynamic"],
                VersionNormal = portalOptions.Value?.Version,
                VersionMinitor = portalOptionsMonitor.CurrentValue?.Version
            });
        }

        [HttpGet("put/{v}")]
        public IActionResult Put(string v)
        {

            //var mem = (configuration as IConfigurationRoot).Providers.FirstOrDefault(c => c.GetType().Name == "MemoryConfigurationProvider") as MemoryConfigurationProvider;
            //mem.Add("AirCC:Version", v);
            //(configuration as IConfigurationRoot).Reload();
            return Ok();
        }

        //public class APIDataService : BackgroundService
        //{
        //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //    {
        //        while (!stoppingToken.IsCancellationRequested)
        //        {
        //            try
        //            {
        //                //需要执行的任务

        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error(ex.Message);
        //            }
        //            await Task.Delay(1000, stoppingToken);//等待1秒
        //        }
        //    }
        //}
    }
}
