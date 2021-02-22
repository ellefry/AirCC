using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.Domain;
using AirCC.Portal.EntityFramework;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace AirCC.Portal.AppService
{
    public class ApplicationConfigurationAppService : ApplicationServiceBase<ApplicationConfiguration, string>, IApplicationConfigurationAppService
    {
        public ApplicationConfigurationAppService(IRepository<ApplicationConfiguration, string> repository, IServiceProvider serviceProvider)
            : base(repository, serviceProvider)
        {
        }

        public async Task Update([NotNull] CreateConfigurationInput input)
        {
            var configuration = Repository.Table.Include(c => c.ConfigurationHistories)
                .FirstOrDefaultAsync(c=>c.Id == input.Id);
        }
    }
}
