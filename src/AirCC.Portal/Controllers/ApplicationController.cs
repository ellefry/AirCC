using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;

namespace AirCC.Portal.Controllers
{
    [Route("api/app")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationAppService applicationAppService;
        private readonly IApplicationConfigurationAppService configurationAppService;

        public ApplicationController(IApplicationAppService applicationAppService, IApplicationConfigurationAppService configurationAppService)
        {
            this.applicationAppService = applicationAppService;
            this.configurationAppService = configurationAppService;
        }

        [HttpPost("Create")]
        public async Task Create(ApplicationInput input)
        {
            await applicationAppService.CreateAsync(input);
        }

        [HttpPost("{appId}/addConfiguration")]

        public async Task AddConfiguration(string appId, CreateConfigurationInput input)
        {
            await applicationAppService.AddConfiguration(appId, input);
        }

    }
}
