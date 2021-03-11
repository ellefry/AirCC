using AirCC.Client;
using AirCC.Client.Registry;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Clients
{
    public interface ISettingsSender
    {
        Task SendSettings(AirCCSettingCollection settings, ApplicationRegistry registry);
        Task SendSettings(AirCCSettingCollection settings, string appId);
    }
}
