using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;
using AirCC.Portal.Domain.DomainServices;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IApplicationService = AirCC.Portal.Domain.DomainServices.IApplicationService;

namespace AirCC.Portal.AppService
{
    public class ApplicationAppService : ApplicationServiceBase<Application,string>, IApplicationAppService
    {
        private readonly IApplicationService applicationService;

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider, 
            IApplicationService applicationService)
            : base(repository, serviceProvider)
        {
            this.applicationService = applicationService;
        }

        public async Task Create([NotNull] ApplicationInput applicationInput)
        {
            var application = MapToEntity(applicationInput);
            await applicationService.Create(application);
        }

        public async Task Update([NotNull] ApplicationInput applicationInput)
        {
            //var application = MapToEntity(applicationInput);
            var application = await GetEntityByIdAsync(applicationInput.Id);
            application = MapToEntity(applicationInput);
            await applicationService.Update(application);
        }

        
    }
}
