using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BCI.Extensions.Core.Dependency;

// ReSharper disable once CheckNamespace
namespace AirCC.Portal.Domain.DomainServices
{
    public interface IApplicationConfigurationService : IScopedDependency
    {
        Task Update(ApplicationConfiguration applicationConfiguration, string oldKey, string oldValue);
    }
}
