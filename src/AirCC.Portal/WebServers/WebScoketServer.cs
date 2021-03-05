using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirCC.Portal.Infrastructure;
using Castle.Core.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WatsonWebsocket;

namespace AirCC.Portal.WebServers
{
    public class  WebScoketServer : BackgroundService
    {
        private readonly ILogger<WebScoketServer> webSocketServer;
        private readonly AirCCWsServer server;
        public WebScoketServer(ILogger<WebScoketServer> webSocketServer, AirCCWsServer server)
        {
            this.webSocketServer = webSocketServer;
            this.server = server;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                server.Start();
            }, stoppingToken);
        }
    }
}
