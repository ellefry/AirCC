using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using BCI.Extensions.DDD.ApplicationService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost("{appId}/updateConfiguration")]
        public async Task UpdateConfiguration(string appId, CreateConfigurationInput input)
        {
            await applicationAppService.UpdateConfigurationValue(appId, input);
        }

        [HttpPost("{appId}/online/{cfgId}")]
        public async Task OnlineConfiguration(string appId, string cfgId)
        {
            await applicationAppService.OnlineConfiguration(appId, cfgId);
        }

        [HttpGet("{appId}/GetConfigurations")]
        public async Task<PagedResultDto<ConfigurationListOutput>> GetPagedConfigurations(string appId, ConfigurationListInput input)
        {
            return await applicationAppService.GetPagedConfigurations(appId, input);
        }
    }
}
