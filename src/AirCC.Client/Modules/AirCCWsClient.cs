using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WatsonWebsocket;

namespace AirCC.Client.Modules
{
    public class AirCcWsClient
    {
        private readonly AirCCConfigOptions airCcConfigOptions;
        private readonly IAirCCSettingsService airCcSettingsService;

        private WatsonWsClient client = null;

        public AirCcWsClient(AirCCConfigOptions airCcConfigOptions, IAirCCSettingsService airCcSettingsService)
        {
            this.airCcConfigOptions = airCcConfigOptions;
            this.airCcSettingsService = airCcSettingsService;
        }

        public void Start()
        {
            var token = GenerateToken();
            var baseUrl = $"ws://{airCcConfigOptions.RegistryServiceUrl}?token={token}&appId={airCcConfigOptions.ApplicationId}"; client = new WatsonWsClient(new Uri(baseUrl));
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceived += MessageReceived;
            client.Start();
        }

        public void Stop() => client.Stop();

        public bool Connected => client?.Connected ?? false;

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Data != null && args.Data.Length > 0)
            {
                using var ms = new MemoryStream(args.Data);
                BinaryFormatter bf = new BinaryFormatter();
                var settings = bf.Deserialize(ms) as AirCCSettingCollection;
                foreach (var item in settings.AirCCSettings)
                {
                    Console.WriteLine($"{item.Key} : {item.Value}");
                }

                airCcSettingsService.Update(settings.AirCCSettings).Wait();
            }
        }

        private void ServerConnected(object sender, EventArgs args)
        {
            
            Console.WriteLine("Server connected");
        }

        private void ServerDisconnected(object sender, EventArgs args)
        {
            Console.WriteLine("Server disconnected");
        }

        private string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(airCcConfigOptions.ApplicationSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = airCcConfigOptions.ApplicationId,
                Expires = DateTimeOffset.UtcNow.AddSeconds(300).UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "AirCC"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
