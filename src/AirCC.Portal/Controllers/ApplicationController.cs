using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using BCI.Extensions.Core.Json;
using BCI.Extensions.DDD.ApplicationService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
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

        [HttpGet("list/{name}/{pageIndex}")]
        public async Task<IEnumerable<ApplicationListOutput>> GetApplications(string name, int pageIndex = 1)
        {
            //return await applicationAppService
            //    .GetPagedListAsync<ApplicationListOutput, ApplicationListInput>
            //    (new ApplicationListInput { CurrentIndex = pageIndex, Name = name });

            try
            {
                var s = @"{
  'DataList': [
    {
                    'Id': '1',
      'Name': 'AirCC',
      'ClientSecret': 'AirCC_Secret'
    }
  ],
  'PageSize': 10,
  'TotalPages': 1,
  'TotalCount': 1,
  'CurrentIndex': 1
}";

                var t = jsonSerializer.Deserialize<PagedResultDto<ApplicationListOutput>>(s);

                var original = await new HttpClient().GetAsync("http://localhost:5000/sample-data/json.json").Result.Content.ReadAsStringAsync();
                var ret = jsonSerializer.Deserialize<PagedResultDto<ApplicationListOutput>>(original);

                var result = await new HttpClient()
                    .GetFromJsonAsync<PagedResultDto<ApplicationListOutput>>("http://localhost:5000/sample-data/json.json");
            }
            catch (System.Exception e)
            {

                throw;
            }
          

            return await applicationAppService.GetListAsync<ApplicationListOutput>();
        }

        [HttpGet("pagedList/{name}/{pageIndex}")]
        public async Task<PagedResultDto<ApplicationListOutput>>GetPagedApplications(string name, int pageIndex = 1)
        {
            var result =  await applicationAppService
                .GetPagedListAsync<ApplicationListOutput, ApplicationListInput>
                (new ApplicationListInput { CurrentIndex = pageIndex, Name = name });

            return result;
        }

    }
}
