using AirCC.Client.Registry;
using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.Domain;
using AirCC.Portal.Domain.DomainServices;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IApplicationService = AirCC.Portal.Domain.DomainServices.IApplicationService;

namespace AirCC.Portal.AppService
{
    public class ApplicationAppService : ApplicationServiceBase<Application,string>, IApplicationAppService
    {
        private readonly IApplicationService applicationService;
        private readonly IRepository<ApplicationConfiguration, string> configurationRepository;
        private readonly IMemoryCache memoryCache;

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider,
            IApplicationService applicationService, IRepository<ApplicationConfiguration, string> configurationRepository, 
            IMemoryCache memoryCache)
            : base(repository, serviceProvider)
        {
            this.applicationService = applicationService;
            this.configurationRepository = configurationRepository;
            this.memoryCache = memoryCache;
        }

        public async Task Create([NotNull] ApplicationInput applicationInput)
        {
            var application = MapToEntity(applicationInput);
            await applicationService.Create(application);
        }

        public async Task Update([NotNull] ApplicationInput applicationInput)
        {
            var application = await GetEntityByIdAsync(applicationInput.Id);
            application = MapToEntity(applicationInput);
            await applicationService.Update(application);
        }

        public async Task CreateConfiguration([NotNull] CreateConfigurationInput input)
        {
            var applicationConfiguration = this.Mapping.Map<CreateConfigurationInput, ApplicationConfiguration>(input);
            await applicationService.AddConfiguration(applicationConfiguration);
        }

        public async Task UpdateConfiguration([NotNull] CreateConfigurationInput input)
        {
            var configuration = await configurationRepository.FindAsync(input.Id);
            configuration = this.Mapping.Map<CreateConfigurationInput, ApplicationConfiguration>(input);
            await applicationService.UpdateConfiguration(configuration);
        }

        public async Task OnlineConfigurations(OnlineInput input)
        {
            await applicationService.OnlineConfigurations(input.ConfigurationIds);
        }

        public async Task OnlinConfiguration([NotNull] string id)
        {
            await applicationService.OnlineConfigurations(new List<string> { id });
        }

        public async Task RevertConfiguration([NotNull] string historyId)
        {
            await applicationService.RevertConfiguration(historyId);
        }

        public async Task DeleteConfiguration([NotNull]string id)
        {
            //await configurationRepository.DeleteAsync(id);
            //update settings
            var configuration = await configurationRepository.FindAsync(id);
            var application = await Repository.FindAsync(configuration.ApplicationId);
            if (memoryCache.TryGetValue(application.Name, out ApplicationRegistry app))
            {

                using var scope = new TransactionScope(scopeOption: TransactionScopeOption.Required,
                    transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead },
                        asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);
                configurationRepository.Delete(id);

                //send request
                scope.Complete();
            }
        }
    }
}
