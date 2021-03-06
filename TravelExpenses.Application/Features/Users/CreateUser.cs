﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Users
{
    public class CreateUser
    {
        public class Command : IRequest<AuthenticatedUserOut>
        {
            public Command(UserRegistration loginDetails)
            {
                LoginDetails = loginDetails;
            }

            public UserRegistration LoginDetails { get; private set; }
        }

        public class Handler : IRequestHandler<Command, AuthenticatedUserOut>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly ITokenGenerator tokenGenerator;

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory,
                IMapper mapper,
                ITokenGenerator tokenGenerator)
            {
                this.context = context;
                this.mapper = mapper;
                this.tokenGenerator = tokenGenerator;
            }

            public async Task<AuthenticatedUserOut> Handle(Command request, CancellationToken cancellationToken)
            {
                var userExists = await context.Users.AnyAsync(u => u.Email == request.LoginDetails.Email).ConfigureAwait(false);
                if (userExists)
                {
                    throw new UserAlreadyExistsException();
                }

                var user = mapper.Map<User>(request.LoginDetails);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.LoginDetails.Password);
                context.Users.Add(user);

                try
                {
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    var userOut = mapper.Map<AuthenticatedUserOut>(user);
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
                    RuleFor(x => x.LoginDetails.Username).NotNull().Length(3, 60);
                    RuleFor(x => x.LoginDetails.Email).NotNull().EmailAddress();
                    RuleFor(x => x.LoginDetails.Password).NotNull().Length(6, 50);
                });
            }
        }
    }
}
