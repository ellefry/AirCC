using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCI.Extensions.Domain;
using JetBrains.Annotations;

namespace AirCC.Portal.Domain.DomainServices
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRepository<Application, string> applicationRepository;

        public ApplicationService()
        {
        }

        public async Task Create([NotNull]Application application)
        {
            var dulicate =
                applicationRepository.NoTrackingTable.FirstOrDefault(a => a.Name == application.Name);
            if (dulicate != null)
                throw new ApplicationException($"Application Name [{application.Name}] existed!");
            await applicationRepository.InsertAsync(application);
        }
    }
}
