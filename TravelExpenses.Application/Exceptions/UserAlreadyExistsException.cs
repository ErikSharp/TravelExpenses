using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("The user already exists")
        {

        }
    }
}
