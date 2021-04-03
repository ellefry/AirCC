using BCI.Extensions.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.DomainServices.Abstract
{
    public interface ICryptoHasher : ISingletonDependency
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string password);
    }

}
