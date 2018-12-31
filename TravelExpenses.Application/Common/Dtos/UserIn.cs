using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class UserIn
    {
        public UserIn(string username, string email, string password)
        {
            var emailTemp = email ?? "";
            Email = emailTemp.ToLowerInvariant();
            Password = password;
            Username = username;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
