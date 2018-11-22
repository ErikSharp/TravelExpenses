using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public const string ExMessage = "The user already exists";

        public UserAlreadyExistsException() : base(ExMessage)
        {

        }
    }
}
