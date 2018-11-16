using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Common;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Application.Interfaces
{
    public interface ITokenGenerator
    {
        string CreateTokenString(User user);
    }
}
