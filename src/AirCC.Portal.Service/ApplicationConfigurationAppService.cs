using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.Domain;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using System;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService
{
    public class ApplicationConfigurationAppService : ApplicationServiceBase<ApplicationConfiguration, string>, IApplicationConfigurationAppService
    {
        public ApplicationConfigurationAppService(IRepository<ApplicationConfiguration, string> repository, IServiceProvider serviceProvider)
            : base(repository, serviceProvider)
        {
        }

        public async Task Revert(string cfgId, string historyId)
        {
            var current = await Repository.FindAsync(cfgId);
            current.RevertFromHistory(historyId);
            await Repository.SaveChangesAsync();
        }
    }
}
