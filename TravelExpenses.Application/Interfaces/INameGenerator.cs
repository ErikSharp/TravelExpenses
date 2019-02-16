using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Interfaces
{
    public interface INameGenerator
    {
        string FirstName();
        string Surname();
        string FullName();
    }
}
