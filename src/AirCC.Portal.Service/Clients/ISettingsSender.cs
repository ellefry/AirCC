using AirCC.Client;
using AirCC.Client.Registry;
using BCI.Extensions.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Clients
{
    public interface ISettingsSender
    {
        Task SendSettings(AirCCSettingCollection settings, ApplicationRegistry registry);
    }
}
