using AirCC.Portal.Domain.DomainServices.Abstract;
using AirCC.Portal.DomainServices.Abstract;
using BCI.Extensions.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AirCC.Portal.Domain.DomainServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<User, string> repository;
        private readonly ICryptoHasher cryptoHasher;
        public UserService(IRepository<User, string> repository)
        {
            this.repository = repository;
        }

        public async Task Create(string username, string password, UserRole role)
        {
            var isDuplicate = (await repository.GetListAsync(u => u.Username.Trim().ToLower() == username.Trim().ToLower())).Any();
            if (isDuplicate) throw new ApplicationException($"{username} already exists!");
            var hashedPassword = cryptoHasher.HashPassword(password);
            var user = User.Create(username, hashedPassword, role);
            await repository.InsertAsync(user);
        }
        
    }
}
