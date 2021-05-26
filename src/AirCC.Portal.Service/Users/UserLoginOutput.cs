using AirCC.Portal.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.Users
{
    public class UserLoginOutput
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
    }
}
