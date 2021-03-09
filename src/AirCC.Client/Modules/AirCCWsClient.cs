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
    public class AirCCWsClient
    {
        private readonly AirCCConfigOptions airCCConfigOptions;
        private readonly IAirCCSettingsService airCcSettingsService;

        public AirCCWsClient(AirCCConfigOptions airCcConfigOptions, IAirCCSettingsService airCcSettingsService)
        {
            airCCConfigOptions = airCcConfigOptions;
            this.airCcSettingsService = airCcSettingsService;
        }

        public void Initialize()
        {
            var token = GenerateToken();
            var baseUrl = $"ws:{airCCConfigOptions.RegistryServiceUrl}?token={token}&appId={airCCConfigOptions.ApplicationId}";
            WatsonWsClient client = new WatsonWsClient(new Uri(baseUrl));
            client.ServerConnected += ServerConnected;
            client.ServerDisconnected += ServerDisconnected;
            client.MessageReceived += MessageReceived;
            client.Start();
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
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
            var key = Encoding.ASCII.GetBytes(airCCConfigOptions.ApplicationSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = airCCConfigOptions.ApplicationId,
                Expires = DateTimeOffset.UtcNow.AddSeconds(300).UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = "AirCC"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token.ToString();
        }
    }
}
