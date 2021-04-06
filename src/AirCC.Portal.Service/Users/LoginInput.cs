using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.Users
{
    public class LoginInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }
}
