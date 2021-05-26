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
        public UserService(IRepository<User, string> repository, ICryptoHasher cryptoHasher)
        {
            this.repository = repository;
            this.cryptoHasher = cryptoHasher;
        }

        public async Task Create(string username, string password, UserRole role)
        {
            var isDuplicate = (await repository.GetListAsync(u => u.Username.Trim().ToLower() == username.Trim().ToLower())).Any();
            if (isDuplicate) throw new ApplicationException($"{username} already exists!");
            var hashedPassword = cryptoHasher.HashPassword(password);
            var user = User.Create(username, hashedPassword, role);
            await repository.InsertAsync(user);
        }

        /// <summary>
        /// validate
        /// </summary>
        /// <param name="username">admin/admin</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> ValidateUser(string username, string password)
        {
            var user = (await repository.GetListAsync(u => u.Username.Trim().ToLower() == username.Trim().ToLower())).FirstOrDefault();
            if (user == null) throw new ApplicationException("Username error!");
            if (!cryptoHasher.VerifyHashedPassword(user.Password, password))
                throw new ApplicationException("Password error!");
            return user;
        }

    }
}
