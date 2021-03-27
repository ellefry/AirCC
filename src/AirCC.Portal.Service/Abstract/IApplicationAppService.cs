using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;
using BCI.Extensions.Core.Dependency;
using BCI.Extensions.DDD.ApplicationService;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Abstract
{
    public interface IApplicationAppService : IApplicationServiceBase<Application, string>, IScopedDependency
    {
        Task AddConfiguration(string appId, [NotNull] CreateConfigurationInput input);
        Task UpdateConfigurationValue(string appId, [NotNull] CreateConfigurationInput input);
        Task OnlineConfiguration(string appId, string cfgId);
        Task<PagedResultDto<ConfigurationListOutput>> GetPagedConfigurations(string appId, ConfigurationListInput input);
        Task Update([NotNull] ApplicationInput applicationInput);
        Task<ConfigurationListOutput> GetConfiguration(string appId, string cfgId);
    }
}
