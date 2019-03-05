using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class UserOut
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public bool Disabled { get; set; }
        public string Preferences { get; set; }
    }
}
