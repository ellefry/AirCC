using BCI.Extensions.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirCC.Portal.Domain
{
    public class User : FullAuditEntity<string>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public static User Create(string username, string password, UserRole role)
        {
            return new User
            {
                Username = username,
                Password = password,
                Role = role
            };
        }
    }

    public enum UserRole
    {
        Admin =0,
        Normal =1
    }
}
