using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;
using AirCC.Portal.Domain.DomainServices;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
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

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider) 
            : base(repository, serviceProvider)
        {
        }

        public async Task Create(ApplicationInput applicationInput)
        
        { 
        }
    }
}
