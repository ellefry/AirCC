using AirCC.Portal.AppService;
using AirCC.Portal.AppService.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AirCC.Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService appService;
        public UserController(IUserAppService appService)
        {
            this.appService = appService;
        }

        [HttpPost("login")]
        public async Task Login([FromBody]LoginInput input)
        {
            if (!await appService.Login(input))
                throw new ApplicationException("Login failed!");
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, input.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
