using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TravelExpenses.Application.Common.Dtos;

namespace TravelExpenses.Application.Common.Validators
{
    public class UserInValidator : AbstractValidator<UserIn>
    {
        public UserInValidator()
        {            
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.Password).NotNull().Length(6, 50);
        }
    }
}
