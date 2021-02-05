using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Client.Apis
{
    [ApiController]
    public class ConfigurationUpdateController
    {

        public ConfigurationUpdateController()
        { 
        }

        [HttpGet("api/aircc/update")]
        public async Task<bool> Update()
        {
            return await Task.FromResult(true);
        }
    }
}
