using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirCC.Client;
using Microsoft.Extensions.Configuration;

namespace NetCoreWeb.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public DynamicController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<KeyValuePair<string,string>> Get()
        {
            return configuration.AsEnumerable().Where(item=>item.Key.Contains("_key"));
        }
    }
}
