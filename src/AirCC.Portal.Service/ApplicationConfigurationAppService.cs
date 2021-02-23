using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.Domain;
using AirCC.Portal.Domain.DomainServices;
using AirCC.Portal.EntityFramework;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AirCC.Portal.AppService
{
    public class ApplicationConfigurationAppService : ApplicationServiceBase<ApplicationConfiguration, string>, IApplicationConfigurationAppService
    {
        private readonly IApplicationConfigurationService configurationService;

        public ApplicationConfigurationAppService(IRepository<ApplicationConfiguration, string> repository, IServiceProvider serviceProvider, IApplicationConfigurationService configurationService)
            : base(repository, serviceProvider)
        {
            this.configurationService = configurationService;
        }

        public async Task Update([NotNull] CreateConfigurationInput input)
        {
            var current = await Repository.Table.Include(c => c.ConfigurationHistories)
                .FirstOrDefaultAsync(c=>c.Id == input.Id);
            string key = current.CfgKey;
            string value = current.CfgValue;
            MapToEntity(input, current);
            await configurationService.Update(current, key, value);
            await Repository.SaveChangesAsync();
        }
    }
}
