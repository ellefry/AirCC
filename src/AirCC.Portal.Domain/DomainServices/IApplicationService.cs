using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.Domain.DomainServices
{
    public interface IApplicationService
    {
        //        Task AddConfiguration([NotNull] ApplicationConfiguration applicationConfiguration);
        Task Create([NotNull] Application application);
        //        Task OnlineConfigurations(IEnumerable<string> configurationIds);
        //Task RevertConfiguration([NotNull] string historyId);
        Task Update([NotNull] Application application);
        //        Task UpdateConfiguration([NotNull] ApplicationConfiguration applicationConfiguration);
    }
}
