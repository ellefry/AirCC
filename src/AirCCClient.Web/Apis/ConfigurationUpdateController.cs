using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Client.Apis
{
    [ApiController]
    public class ConfigurationUpdateController : ControllerBase
    {
        private readonly IAirCCSettingsService airCCSettingsService;
        public ConfigurationUpdateController(IAirCCSettingsService airCCSettingsService)
        {
            this.airCCSettingsService = airCCSettingsService;
        }

        [HttpPost("api/aircc/update")]
        public async Task Update([FromBody]AirCCSettingCollection airCCSettingCollection)
        {
            await airCCSettingsService.Update(airCCSettingCollection?.AirCCSettings);
        }
    }
}
