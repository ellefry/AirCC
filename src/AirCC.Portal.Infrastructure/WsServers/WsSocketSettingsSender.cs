using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AirCC.Client;
using AirCC.Client.Registry;
using AirCC.Portal.AppService.Clients;

namespace AirCC.Portal.Infrastructure.WsServers
{
    public class WsSocketSettingsSender : ISettingsSender
    {
        private readonly AirCCWsServer airCcWsServer;
        public WsSocketSettingsSender(AirCCWsServer airCcWsServer)
        {
            this.airCcWsServer = airCcWsServer;
        }

        public Task SendSettings(AirCCSettingCollection settings, ApplicationRegistry registry)
        {
            throw new NotImplementedException();
        }

        public async Task SendSettings(AirCCSettingCollection settings, string appId)
        {
            var app = airCcWsServer.GetApplicationIpPort(appId);
            BinaryFormatter bf = new BinaryFormatter();
            await using var ms = new MemoryStream();
            bf.Serialize(ms,settings);
            await airCcWsServer.SendAsync(app, ms.ToArray());
        }

    }
}
