using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class UserIn
    {
        public UserIn(string email, string password)
        {
            var emailTemp = email ?? "";
            Email = emailTemp.ToLowerInvariant();
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; private set; }
    }
}
