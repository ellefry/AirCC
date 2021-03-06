﻿using BCI.Extensions.Core.Dependency;
using BCI.Extensions.Domain;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirCC.Portal.Domain.DomainServices
{
    public class ApplicationService : IApplicationService, IScopedDependency
    {
        private readonly IRepository<Application, string> applicationRepository;

        public ApplicationService(IRepository<Application, string> applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public async Task Create([NotNull] Application application)
        {
            var duplicate =
                applicationRepository.NoTrackingTable.FirstOrDefault(a => a.Name == application.Name);
            if (duplicate != null)
                throw new ApplicationException($"Duplicate application name [{application.Name}]!");
            await applicationRepository.InsertAsync(application);
        }

        public async Task Update([NotNull] Application application)
        {
            var duplicate = applicationRepository.NoTrackingTable
                .FirstOrDefault(a => a.Id != application.Id && a.Name == application.Name);
            if (duplicate != null)
                throw new ApplicationException($"Duplicate application name [{application.Name}]!");
            await applicationRepository.UpdateAsync(application);
        }

    }

    //    public class ApplicationService : IApplicationService, IScopedDependency
    //    {
    //        private readonly IRepository<Application, string> applicationRepository;
    //        private readonly IRepository<ApplicationConfiguration, string> configurationRepository;
    //        private readonly IRepository<ApplicationConfigurationHistory, string> historyRepository;

    //        public ApplicationService()
    //        {
    //        }

    //        public async Task Create([NotNull]Application application)
    //        {
    //            var duplicate =
    //                applicationRepository.NoTrackingTable.FirstOrDefault(a => a.Name == application.Name);
    //            if (duplicate != null)
    //                throw new ApplicationException($"Duplicated application name [{application.Name}]!");
    //            await applicationRepository.InsertAsync(application);
    //        }

    //        public async Task Update([NotNull] Application application)
    //        {
    //            var duplicate = applicationRepository.NoTrackingTable
    //                .FirstOrDefault(a => a.Id != application.Id && a.Name == application.Name);
    //            if (duplicate != null)
    //                throw new ApplicationException($"Duplicated application name [{application.Name}]!");
    //            await applicationRepository.UpdateAsync(application);
    //        }

    //        public async Task AddConfiguration([NotNull] ApplicationConfiguration applicationConfiguration)
    //        {
    //            var duplicate = configurationRepository.NoTrackingTable
    //                .FirstOrDefault(c => c.CfgKey == applicationConfiguration.CfgKey 
    //                    && c.ApplicationId == applicationConfiguration.ApplicationId);
    //            if (duplicate != null)
    //                throw new ApplicationException($"Duplicated configuration key [{applicationConfiguration.CfgKey}]!");
    //            await configurationRepository.InsertAsync(applicationConfiguration);
    //        }

    //        public async Task UpdateConfiguration([NotNull] ApplicationConfiguration applicationConfiguration)
    //        {
    //            var duplicate = configurationRepository.NoTrackingTable
    //               .FirstOrDefault(c => c.Id != applicationConfiguration.Id && c.CfgKey == applicationConfiguration.CfgKey
    //                    && c.ApplicationId == applicationConfiguration.ApplicationId);
    //            if (duplicate != null)
    //                throw new ApplicationException($"Duplicated configuration key [{applicationConfiguration.CfgKey}]!");

    //            //get current
    //            var current = configurationRepository.NoTrackingTable.FirstOrDefault(c => c.Id == applicationConfiguration.Id);
    //            var history = new ApplicationConfigurationHistory
    //            {
    //                ApplicationId = current.ApplicationId,
    //                ConfigurationId = applicationConfiguration.Id,
    //                CfgKey = current.CfgKey,
    //                CfgValue = current.CfgValue
    //            };
    //            current.Offline();
    //            await historyRepository.InsertAsync(history);
    //            await configurationRepository.UpdateAsync(applicationConfiguration);
    //        }

    //        public async Task OnlineConfigurations(IEnumerable<string> configurationIds)
    //        {
    //            var configs = configurationRepository.GetQueryable(c => configurationIds.Contains(c.Id));
    //            foreach (var c in configs)
    //            {
    //                c.Online();
    //            }
    //            await configurationRepository.UpdateAsync(configs);
    //        }

    //        public async Task RevertConfiguration([NotNull]string historyId)
    //        {
    //            var history = await historyRepository.FindAsync(historyId);
    //            var current = await configurationRepository.FindAsync(history.ConfigurationId);
    //            current.Renew(history.CfgKey, history.CfgValue);
    //            await UpdateConfiguration(current);
    //        }
    //    }
}
