﻿using AirCC.Portal.AppService.Abstract;
using AirCC.Portal.AppService.Users;
using AirCC.Portal.Domain;
using AirCC.Portal.Domain.DomainServices.Abstract;
using BCI.Extensions.Core.Dependency;
using BCI.Extensions.DDD.ApplicationService;
using BCI.Extensions.Domain;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirCC.Portal.AppService
{
    public interface IUserAppService : IApplicationServiceBase<User, string>, IScopedDependency
    {
        Task<UserLoginOutput> Login([NotNull] LoginInput loginDto);
    }

    public class UserAppService : ApplicationServiceBase<User, string>, IUserAppService
    {
        private readonly IUserService userService;
        public UserAppService(IRepository<User, string> repository, IServiceProvider serviceProvider, IUserService userService)
            : base(repository, serviceProvider)
        {
            this.userService = userService;
        }

        public async Task CreateAdmin([NotNull]CreateUserInput createAdminInput)
        {
            if (!createAdminInput.IsPasswordValid())
                throw new ApplicationException("Password and confirm password mismatch!");
            await userService.Create(createAdminInput.Username, createAdminInput.Password, UserRole.Admin);
            await Repository.SaveChangesAsync();
        }

        public async Task CreateNormalUser([NotNull] CreateUserInput createAdminInput)
        {
            if (!createAdminInput.IsPasswordValid())
                throw new ApplicationException("Password and confirm password mismatch!");
            await userService.Create(createAdminInput.Username, createAdminInput.Password, UserRole.Normal);
            await Repository.SaveChangesAsync();
            //
        }

        public async Task<UserLoginOutput> Login([NotNull] LoginInput loginDto)
        {
            var user = await userService.ValidateUser(loginDto.Username, loginDto.Password);
            return new UserLoginOutput
            {
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<bool> HasAnyUser()
        {
            return await Repository.Table.CountAsync() > 0;
        }

    }
}
