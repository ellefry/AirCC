using AirCC.Client;
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
using System.Linq;
using IApplicationService = AirCC.Portal.Domain.DomainServices.IApplicationService;
using AirCC.Portal.AppService.Clients;

namespace AirCC.Portal.AppService
{
    public class ApplicationAppService : ApplicationServiceBase<Application,string>, IApplicationAppService
    {
        private readonly IApplicationService applicationService;
        private readonly IRepository<ApplicationConfiguration, string> configurationRepository;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingsSender settingsSender;

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider,
            IApplicationService applicationService, IRepository<ApplicationConfiguration, string> configurationRepository,
            IMemoryCache memoryCache, ISettingsSender settingsSender)
            : base(repository, serviceProvider)
        {
            this.applicationService = applicationService;
            this.configurationRepository = configurationRepository;
            this.memoryCache = memoryCache;
            this.settingsSender = settingsSender;
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
            var configuration = await configurationRepository.FindAsync(id);
            var application = await Repository.FindAsync(configuration.ApplicationId);


            using var scope = new TransactionScope(scopeOption: TransactionScopeOption.Required,
                transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead },
                    asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);
            configurationRepository.Delete(id);
            UpdateClientSettings(application.Id).Wait();
            scope.Complete();


        }

        private async Task<AirCCSettingCollection> GetConfigurations([NotNull] string applicationId)
        {
            var configurations = await configurationRepository.GetListAsync(c => c.ApplicationId == applicationId);
            var settings = configurations.Select(c => new AirCCSetting { Key = c.CfgKey, Value = c.CfgValue });
            return new AirCCSettingCollection { AirCCSettings = settings.ToList() };
        }

        private async Task<ApplicationRegistry> GetApplicationRegistry(string applicationId)
        {
            var application = await Repository.FindAsync(applicationId);
            if (memoryCache.TryGetValue(application.Name, out ApplicationRegistry app))
            {
                return app;
            }
            throw new ApplicationException($"Can't find registry by application [{application.Name}]");
        }

        public async Task UpdateClientSettings(string applicationId, Action<string> action = null)
        {
            var settings = await GetConfigurations(applicationId);
            var registry = await GetApplicationRegistry(applicationId);
            await settingsSender.SendSettings(settings, registry);
        }
    }
}
