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
using AirCC.Portal.EntityFramework;
using AutoMapper;
using BCI.Extensions.Core.ObjectMapping;
using Microsoft.EntityFrameworkCore;

namespace AirCC.Portal.AppService
{
    public class ApplicationAppService : ApplicationServiceBase<Application, string>, IApplicationAppService
    {
        private readonly IApplicationService applicationService;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingsSender settingsSender;
        private readonly AirCCDbContext dbContext;
        private readonly IRepository<ApplicationConfiguration> configurationRepository;

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider,
            IApplicationService applicationService,
            IMemoryCache memoryCache, ISettingsSender settingsSender, AirCCDbContext dbContext, IEntityMappingManager m,
            IMapper mapper, IRepository<ApplicationConfiguration> configurationRepository)
            : base(repository, serviceProvider)
        {
            this.applicationService = applicationService;
            this.memoryCache = memoryCache;
            this.settingsSender = settingsSender;
            this.dbContext = dbContext;
            this.configurationRepository = configurationRepository;
        }

        public override async Task CreateAsync<TCreateInput>(TCreateInput input)
        {
            var application = input as ApplicationInput;
            var newApplication = this.Mapping.Map<ApplicationInput, Application>(application);
            await applicationService.Create(newApplication);
            await Repository.SaveChangesAsync();
        }

        public async Task Update([NotNull] ApplicationInput applicationInput)
        {
            var application = await GetEntityByIdAsync(applicationInput.Id);
            MapToEntity(applicationInput, application);
            await applicationService.Update(application);
            await Repository.SaveChangesAsync();
        }

        public async Task AddConfiguration(string appId, [NotNull] CreateConfigurationInput input)
        {
            var applicationConfiguration = this.Mapping.Map<CreateConfigurationInput, ApplicationConfiguration>(input);
            var application = await Repository.Table.FirstOrDefaultAsync(a => a.Id == appId);
            application.AddConfiguration(applicationConfiguration);
            await Repository.SaveChangesAsync();
        }

        public async Task UpdateConfigurationValue(string appId, [NotNull] CreateConfigurationInput input)
        {
            var application = await Repository.Table.FirstOrDefaultAsync(a => a.Id == appId);
            application.UpdateConfigurationValue(input.Id, input.CfgValue);
            await Repository.SaveChangesAsync();
        }

        public async Task OnlineConfiguration(string appId, string cfgId)
        {
            var application = await Repository.FindAsync(appId);
            application.Online(cfgId);
            await ExecuteTransaction(UpdateClientSettings, application);
        }

        public async Task OfflineConfiguration(string appId, string cfgId)
        {
            var application = await Repository.FindAsync(appId);
            application.Offline(cfgId);
            await ExecuteTransaction(UpdateClientSettings, application);
        }

        public async Task DeleteConfiguration(string appId, string cfgId)
        {
            var application = await Repository.FindAsync(appId);
            application.DeleteConfiguration(cfgId);
            await ExecuteTransaction(UpdateClientSettings, application);
        }

        public async Task<PagedResultDto<ConfigurationListOutput>> GetPagedConfigurations(string appId,
            ConfigurationListInput input)
        {
            var application = await Repository.FindAsync(appId);
            return await this.Mapping.Map<ConfigurationListOutput>(application.GetConfigurations().AsQueryable())
                .ToPageAsync(input.CurrentIndex, input.PageSize, "Id");
        }

        private ApplicationRegistry GetRegisterApplication(string name)
        {
            if (memoryCache.TryGetValue(name, out ApplicationRegistry app))
            {
                return app;
            }

            //return new ApplicationRegistry { Id = "AirCC" };
            throw new ApplicationException($"Can't find registry by application [{name}]");
        }

        private async Task UpdateClientSettings(Application application)
        {
            var settings = application.GetConfigurations()
                .Select(c => new AirCCSetting {Key = c.CfgKey, Value = c.CfgValue});
            var airCCSettingCollection = new AirCCSettingCollection {AirCCSettings = settings.ToList()};
            var registry = GetRegisterApplication(application.Name);
            await settingsSender.SendSettings(airCCSettingCollection, registry);
        }

        private async Task ExecuteTransaction(Func<Application, Task> action, Application application)
        {
            using var scope = new TransactionScope(scopeOption: TransactionScopeOption.Required,
                transactionOptions: new TransactionOptions {IsolationLevel = IsolationLevel.RepeatableRead},
                asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);
            await Repository.SaveChangesAsync();
            await action(application);
            scope.Complete();
        }

    }
}

    
