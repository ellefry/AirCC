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
            IOptionsMonitor<AirCCModel> portalOptionsMonitor)
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
    }
}
