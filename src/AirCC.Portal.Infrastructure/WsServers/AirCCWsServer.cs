using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AirCC.Portal.Domain;
using BCI.Extensions.Core.Extensions;
using BCI.Extensions.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WatsonWebsocket;

namespace AirCC.Portal.Infrastructure
{
    public class AirCCWsServer : WatsonWsServer
    {
        public const string ListeningPort = "WsServerPort";

        private static readonly ConcurrentDictionary<string, string> aicCcClients =
            new ConcurrentDictionary<string, string>();

        private readonly IServiceProvider sp;
        private readonly ILogger<AirCCWsServer> logger;


        public AirCCWsServer(string host, int port, IServiceProvider sp)
            : base(host, port, false)
        {
            base.ClientConnected += ClientConnected;
            base.ClientDisconnected += ClientDisconnected;
            base.MessageReceived += MessageReceived;
            this.sp = sp;
            this.logger = sp.GetRequiredService<ILogger<AirCCWsServer>>();
        }

        public string GetApplicationIpPort(string appId)
        {
            if (aicCcClients.TryGetValue(appId, out var ipPort))
                return ipPort;
            return null;
        }

        private void ClientConnected(object sender, ClientConnectedEventArgs args)
        {
            Console.WriteLine("Client connected: " + args.IpPort);
            var nameValues = HttpUtility.ParseQueryString(args.HttpRequest.Url.Query);
            string appId = string.Empty;
            var token = string.Empty;
            if (nameValues.TryGetValue("appId", out string _appId))
            {
                appId = _appId;
            }

            if (nameValues.TryGetValue("token", out string _token))
            {
                token = _token;
            }

            var isValid = ValidateToken(token, appId);
            if (!isValid)
            {
                this.DisconnectClient(args.IpPort);
            }

            UpdateClient(appId, args.IpPort);
        }

        private void ClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            Console.WriteLine("Client disconnected: " + args.IpPort);
            var item = aicCcClients.FirstOrDefault(kv => kv.Value == args.IpPort);
            if (item.Key != null)
                aicCcClients.TryRemove(item.Key, out _);
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message received from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        private void UpdateClient(string appId, string ipPort)
        {
            if (!aicCcClients.ContainsKey(appId))
            {
                aicCcClients.TryAdd(appId, ipPort);
            }
            else
            {
                if (aicCcClients.TryUpdate(appId, ipPort, aicCcClients[appId]))
                {
                    //if replaced, disconnect old connection
                    this.DisconnectClient(aicCcClients[appId]);
                }
            }
        }

        private bool ValidateToken(string token, string appId)
        {
            try
            {
                using var scope = sp.CreateScope();
                var appRepo = scope.ServiceProvider.GetRequiredService<IRepository<Application, string>>();
                var application = appRepo.NoTrackingTable.FirstOrDefault(a => a.Name == appId);
                if (application == null)
                    throw new ApplicationException($"No application [{appId}]");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(application.ClientSecret);
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = appId,
                    ValidAudience = "AirCC",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message + Environment.NewLine + e.StackTrace);
                return false;
            }
        }
    }
}
