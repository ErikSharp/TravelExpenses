using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Common.Validators;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features
{
    public class CreateUser
    {
        public class Command : IRequest<UserOut>
        {
            public Command(UserIn loginDetails)
            {
                LoginDetails = loginDetails;
            }

            public UserIn LoginDetails { get; private set; }
        }

        public class Handler : IRequestHandler<Command, UserOut>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly ITokenGenerator tokenGenerator;
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory,
                IMapper mapper,
                ITokenGenerator tokenGenerator)
            {
                this.context = context;
                this.mapper = mapper;
                this.tokenGenerator = tokenGenerator;
                this.logger = loggerFactory.CreateLogger(nameof(Handler));
            }

            public async Task<UserOut> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = mapper.Map<User>(request.LoginDetails);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.LoginDetails.Password);
                context.Users.Add(user);

                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    var userOut = mapper.Map<UserOut>(user);
                    userOut.Token = tokenGenerator.CreateTokenString(user);

                    return userOut;
                }
                catch (DbUpdateException ex)
                {
                    Exception resultEx = ex;
                    var sex = ex.InnerException as SqlException;
                    if (sex != null)
                    {
                        switch (sex.Number)
                        {
                            case 2601:
                                resultEx = new UserAlreadyExistsException();
                                break;
                        }
                    }

                    throw resultEx;                    
                }                
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.LoginDetails).NotNull().DependentRules(() =>
                {
                    RuleFor(x => x.LoginDetails).SetValidator(new UserInValidator());
                });
            }
        }
    }
}
