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
    }

    //    public interface IApplicationAppService : IScopedDependency
    //    {
    //        Task Create([NotNull] ApplicationInput applicationInput);
    //        Task CreateConfiguration([NotNull] CreateConfigurationInput input);
    //        Task OnlinConfiguration(string Id);
    //        Task OnlineConfigurations(OnlineInput input);
    //        Task RevertConfiguration(string historyId);
    //        Task Update([NotNull] ApplicationInput applicationInput);
    //        Task UpdateConfiguration([NotNull] CreateConfigurationInput input);
    //    }
}
