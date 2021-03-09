using AirCC.Client;
using AirCC.Client.Registry;
using AirCC.Portal.AppService.Clients;
using BCI.Extensions.Core.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.Infrastructure
{
    public class HttpSettingSender : ISettingsSender
    {
        private readonly IJsonSerializer jsonSerializer;

        public HttpSettingSender(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public async Task SendSettings(AirCCSettingCollection settings, ApplicationRegistry registry)
        {
            var content = jsonSerializer.Serialize(settings);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            await new HttpClient().PostAsync(registry.Url + "/api/aircc/update", httpContent);
        }

        public Task SendSettings(AirCCSettingCollection settings, string appId)
        {
            throw new NotImplementedException();
        }
    }
}
