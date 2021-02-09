using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCCClient.Web.Apis;

namespace AirCC.Client.Apis
{
    [ApiController]
    public class ConfigurationUpdateController : ControllerBase
    {
        private readonly IAirCCSettingsService airCCSettingsService;
        private readonly IUpdateAuthorizationService authorizationService;
        public ConfigurationUpdateController(IAirCCSettingsService airCCSettingsService, 
            IUpdateAuthorizationService authorizationService)
        {
            this.airCCSettingsService = airCCSettingsService;
            this.authorizationService = authorizationService;
        }

        [HttpPost("api/aircc/update")]
        public async Task Update([FromBody]AirCCSettingCollection airCCSettingCollection)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ')?.Last();
            if (authorizationService.Validate(token))
            {
                await airCCSettingsService.Update(airCCSettingCollection?.AirCCSettings);
            }
            else
            {
                await Task.FromResult(new BadRequestResult());
            }
        }
    }
}
