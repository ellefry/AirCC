using AirCC.Portal.AppService.Abstract;
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
        private readonly IApplicationConfigurationAppService configurationAppService;

        public ConfigurationsController(IApplicationConfigurationAppService configurationAppService)
        {
            this.configurationAppService = configurationAppService;
        }

        [HttpPost("{cfgId}/Revert/{historyId}")]
        public async Task Revert(string cfgId, string historyId)
        {
            await configurationAppService.Revert(cfgId, historyId);
        }
    }
}
