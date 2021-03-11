using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<KeyValuePair<string, string>> Get()
        {
            return configuration.AsEnumerable().Where(item => item.Key.Contains("_key"));
        }
    }
}
