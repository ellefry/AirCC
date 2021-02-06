using AirCC.Client.Registry;
using AirCC.Portal.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistryController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IApplicationRegistryService applicationRegistryService;

        public RegistryController(IMemoryCache memoryCache, IApplicationRegistryService applicationRegistryService)
        {
            this.memoryCache = memoryCache;
            this.applicationRegistryService = applicationRegistryService;
        }

        [HttpPost("register")]
        public async Task RegisterApplication(ApplicationRegistry applicationRegistry)
        {
            applicationRegistryService.AddApplication(applicationRegistry);
            await Task.CompletedTask;
        }
    }
}
