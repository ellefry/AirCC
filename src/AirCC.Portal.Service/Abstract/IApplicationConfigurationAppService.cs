using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AirCC.Portal.Domain;
using BCI.Extensions.Core.Dependency;
using BCI.Extensions.DDD.ApplicationService;

namespace AirCC.Portal.AppService.Abstract
{
    public interface IApplicationConfigurationAppService : IApplicationServiceBase<ApplicationConfiguration, string>, IScopedDependency
    {
        Task Revert(string cfgId, string historyId);
    }
}
