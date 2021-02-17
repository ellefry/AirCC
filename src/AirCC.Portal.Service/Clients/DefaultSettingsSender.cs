using AirCC.Client;
using AirCC.Client.Registry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Clients
{
    public class DefaultSettingsSender : ISettingsSender
    {
        public async Task SendSettings(AirCCSettingCollection settings, ApplicationRegistry registry)
        {
            await Task.CompletedTask;
        }
    }
}
