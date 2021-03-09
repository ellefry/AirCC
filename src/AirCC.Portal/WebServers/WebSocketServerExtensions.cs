﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCC.Portal.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatsonWebsocket;

namespace AirCC.Portal.WebServers
{
    public static class WebSocketServerExtensions
    {
        public static void AddWebSocketServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new AirCCWsServer("localhost", 4999,services.BuildServiceProvider()));
            services.AddHostedService<WebScoketServer>();
        }
    }
}
