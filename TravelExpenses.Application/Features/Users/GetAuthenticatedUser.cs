using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Users
{
    public class GetAuthenticatedUser
    {
        public class Query : IRequest<UserOut>
        {
            public Query(UserIn loginDetails)
            {
                LoginDetails = loginDetails;
            }

            public UserIn LoginDetails { get; private set; }
        }

        public class Handler : IRequestHandler<Query, UserOut>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly ITokenGenerator tokenGenerator;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper,
                ITokenGenerator tokenGenerator)
            {
                this.context = context;
                this.mapper = mapper;
                this.tokenGenerator = tokenGenerator;
            }

            public async Task<UserOut> Handle(Query request, CancellationToken cancellationToken)
            {
                Log.Debug($"Looking for not disabled user with email: {request.LoginDetails.Email}");

                var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(x => 
                    x.Email == request.LoginDetails.Email)
                    .ConfigureAwait(false);
                
                if (user == null)
                {
                    Log.Debug($"User {request.LoginDetails.Email} was not found");
                    return null;
                }
                else
                {
                    Log.Debug($"User {request.LoginDetails.Email} was found");
                }

                bool passMatchesHash =
                    BCrypt.Net.BCrypt.Verify(request.LoginDetails.Password, user.PasswordHash);

                if (!passMatchesHash)
                {
                    Log.Information($"Login for {request.LoginDetails.Email} used the wrong password");
                    return null;
                }
                else
                {
                    Log.Debug($"Login for {request.LoginDetails.Email} had the correct password");
                }

                return CreateUserWithToken(user);                
            }

            private UserOut CreateUserWithToken(User user)
            {
                var userOut = mapper.Map<UserOut>(user);
                userOut.Token = tokenGenerator.CreateTokenString(user);

                return userOut;
            }
        }     
    }
}
