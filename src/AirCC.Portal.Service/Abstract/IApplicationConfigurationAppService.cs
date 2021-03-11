using AirCC.Portal.Domain;
using BCI.Extensions.Core.Dependency;
using BCI.Extensions.DDD.ApplicationService;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Abstract
{
    public interface IApplicationConfigurationAppService : IApplicationServiceBase<ApplicationConfiguration, string>, IScopedDependency
    {
        Task Revert(string cfgId, string historyId);
    }
}
