using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.AppService.Users
{
    public class CreateUserInput
    {
        public string Username { get; set; } = "admin";
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool IsPasswordValid() => !string.IsNullOrEmpty(Password) && Password == ConfirmPassword;
    }
}
