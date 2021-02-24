using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.Extensions.Domain;

namespace AirCC.Portal.Domain.DomainServices
{
    public class ApplicationConfigurationService : IApplicationConfigurationService
    {
        private readonly IRepository<ApplicationConfiguration, string> repository;
        //private readonly IRepository<ApplicationConfigurationHistory, string> historyRepository
        public ApplicationConfigurationService(IRepository<ApplicationConfiguration, string> repository)
        {
            this.repository = repository;
        }

        public async Task Update(ApplicationConfiguration applicationConfiguration, string oldKey, string oldValue)
        {
            var duplicate = repository.NoTrackingTable
               .FirstOrDefault(c => c.Id != applicationConfiguration.Id && c.CfgKey == applicationConfiguration.CfgKey);
            if (duplicate != null) 
                throw new ApplicationException($"Duplicated configuration key [{applicationConfiguration.CfgKey}]!");
            await repository.UpdateAsync(applicationConfiguration);
        }
    }
}
