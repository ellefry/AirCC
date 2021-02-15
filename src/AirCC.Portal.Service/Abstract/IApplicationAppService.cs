﻿using AirCC.Portal.AppService.ApplicationDtos;
using BCI.Extensions.Core.Dependency;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService.Abstract
{
    public interface IApplicationAppService : IScopedDependency
    {
        Task Create([NotNull] ApplicationInput applicationInput);
        Task CreateConfiguration([NotNull] CreateConfigurationInput input);
        Task OnlinConfiguration(string Id);
        Task OnlineConfigurations(OnlineInput input);
        Task RevertConfiguration(string historyId);
        Task Update([NotNull] ApplicationInput applicationInput);
        Task UpdateConfiguration([NotNull] CreateConfigurationInput input);
    }
}