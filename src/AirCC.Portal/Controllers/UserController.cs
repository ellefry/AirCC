using AirCC.Portal.AppService.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly IPasswordHasher passwordHasher;
        public UserController()
        { }

        [HttpPost]
        public async Task CreateAdmin(CreateUserInput input)
        { 
        }
    }
}
