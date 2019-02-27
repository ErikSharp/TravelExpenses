using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Common.Dtos
{
    public class AuthenticatedUserOut
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
