using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper,
                ITokenGenerator tokenGenerator,
                ILoggerFactory loggerFactory)
            {
                this.context = context;
                this.mapper = mapper;
                this.tokenGenerator = tokenGenerator;
                this.logger = loggerFactory.CreateLogger(nameof(Handler));
            }

            public async Task<UserOut> Handle(Query request, CancellationToken cancellationToken)
            {
                logger.LogDebug($"Looking for not disabled user with email: {request.LoginDetails.Email}");

                var user = await context.Users.AsNoTracking().SingleOrDefaultAsync(x => 
                    x.Email == request.LoginDetails.Email)
                    .ConfigureAwait(false);
                
                if (user == null)
                {
                    logger.LogDebug($"User {request.LoginDetails.Email} was not found");
                    return null;
                }
                else
                {
                    logger.LogDebug($"User {request.LoginDetails.Email} was found");
                }

                bool passMatchesHash =
                    BCrypt.Net.BCrypt.Verify(request.LoginDetails.Password, user.PasswordHash);

                if (!passMatchesHash)
                {
                    logger.LogInformation($"Login for {request.LoginDetails.Email} used the wrong password");
                    return null;
                }
                else
                {
                    logger.LogDebug($"Login for {request.LoginDetails.Email} had the correct password");
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
