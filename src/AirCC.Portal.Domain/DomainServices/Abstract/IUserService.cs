using BCI.Extensions.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.Domain.DomainServices.Abstract
{
    public interface IUserService : IScopedDependency
    {
        Task Create(string username, string password, UserRole role);
    }
}
