using AirCC.Portal.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AirCC.Portal.WebServers
{
    public class WebScoketServer : BackgroundService
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
