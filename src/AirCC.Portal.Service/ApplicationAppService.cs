using AirCC.Client;
using AirCC.Client.Registry;
using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.ApplicationDtos;
using AirCC.Portal.AppService.Clients;
using AirCC.Portal.Domain;
using AirCC.Portal.EntityFramework;
using BCI.Extensions.Core.ObjectMapping;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IApplicationService = AirCC.Portal.Domain.DomainServices.IApplicationService;

namespace AirCC.Portal.AppService
{
    public class ApplicationAppService : ApplicationServiceBase<Application, string>, IApplicationAppService
    {
        private readonly IApplicationService applicationService;
        private readonly ISettingsSender settingsSender;

        public ApplicationAppService(IRepository<Application, string> repository, IServiceProvider serviceProvider,
            IApplicationService applicationService,ISettingsSender settingsSender)
            : base(repository, serviceProvider)
        {
            this.applicationService = applicationService;
            this.settingsSender = settingsSender;
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

        protected override IQueryable<Application> CreateFilteredQuery<TGetListInput>(TGetListInput input)
        {
            var inputParams = input as ApplicationListInput;
            if (!string.IsNullOrEmpty(inputParams.Name))
            {
                return Repository.NoTrackingTable.Where(a => a.Name.Contains(inputParams.Name));
            }
            return base.CreateFilteredQuery(input);
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

        private async Task UpdateClientSettings(Application application)
        {
            var settings = application.GetConfigurations()
                .Select(c => new AirCCSetting { Key = c.CfgKey, Value = c.CfgValue });
            var airCCSettingCollection = new AirCCSettingCollection { AirCCSettings = settings.ToList() };
            await settingsSender.SendSettings(airCCSettingCollection, application.Name);
        }

        private async Task ExecuteTransaction(Func<Application, Task> action, Application application)
        {
            using var scope = new TransactionScope(scopeOption: TransactionScopeOption.Required,
                transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead },
                asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);
            await Repository.SaveChangesAsync();
            await action(application);
            scope.Complete();
        }

    }
}


