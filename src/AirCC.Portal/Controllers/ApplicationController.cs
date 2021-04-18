using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using BCI.Extensions.Core.Json;
using BCI.Extensions.DDD.ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
    [Authorize]
    [Route("api/app")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationAppService applicationAppService;
        private readonly IJsonSerializer jsonSerializer;

        public ApplicationController(IApplicationAppService applicationAppService, IJsonSerializer jsonSerializer)
        {
            this.applicationAppService = applicationAppService;
            this.jsonSerializer = jsonSerializer;
        }

        [HttpPost("Create")]
        public async Task CreateApplication(ApplicationInput input)
        {
            await applicationAppService.CreateAsync(input);
        }

        [HttpGet("{appId}")]
        public async Task<ApplicationInput> GetApplication(string appId)
        {
            return await applicationAppService.GetAsync<ApplicationInput>(appId);
        }

        [HttpPost("Update")]
        public async Task UpdateApplication(ApplicationInput input)
        {
            await applicationAppService.Update(input);
        }

        [HttpDelete("{appId}")]
        public async Task DeleteApplication(string appId)
        {
            await applicationAppService.DeleteAsync(appId);
        }

        [HttpGet("{appId}/Configuration/{cfgId}")]
        public async Task<ConfigurationListOutput> GetConfiguration(string appId, string cfgId)
        {
            return await applicationAppService.GetConfiguration(appId, cfgId);
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
        public async Task<PagedResultDto<ConfigurationListOutput>> GetPagedConfigurations(string appId,
            [FromQuery] string key, [FromQuery] int pageIndex = 1)
        {
            var input = new ConfigurationListInput { CfgKey = key, CurrentIndex = pageIndex };
            return await applicationAppService.GetPagedConfigurations(appId, input);
        }

        [HttpGet("list/{name}/{pageIndex}")]
        public async Task<IEnumerable<ApplicationListOutput>> GetApplications(string name, int pageIndex = 1)
        {
            return await applicationAppService.GetListAsync<ApplicationListOutput>();
        }

        [HttpGet("pagedList")]
        public async Task<PagedResultDto<ApplicationListOutput>>GetPagedApplications([FromQuery]string name, [FromQuery] int pageIndex = 1)
        {
            var result =  await applicationAppService
                .GetPagedListAsync<ApplicationListOutput, ApplicationListInput>
                (new ApplicationListInput { CurrentIndex = pageIndex, Name = name });

            return result;
        }

    }
}
