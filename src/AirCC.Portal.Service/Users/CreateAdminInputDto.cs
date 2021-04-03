using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.Users
{
    public class CreateAdminInput
    {
        public string Username { get; set; } = "admin";
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool Valid() => !string.IsNullOrEmpty(Password) && Password == ConfirmPassword;
    }
}
