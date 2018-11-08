using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Application.Features
{
    public class UserIn
    {
        //[EmailAddress]
        public string Email { get; set; }
        //[StringLength(50, MinimumLength = 6, ErrorMessage = "The password must be between 6 and 50 characters")]
        public string Password { get; set; }
    }

    public class UserOut
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class AuthenticateUser : IRequest<UserOut>
    {
        public UserIn LoginDetails { get; set; }
    }

    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUser, UserOut>
    {
        private readonly AppSettings appSettings;
        private readonly IMapper mapper;

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Email = "erik.sharp@hadleyshope.com",
                PasswordHash = "$2y$12$yVYkJsR7a4Wj3wRzCD9Pn.DvDGWY3Dzx2AwisSqailn3Pyu.X.zWi" //password
            }
        };

        public AuthenticateUserHandler(
            IOptions<AppSettings> appSettings,
            IMapper mapper)
        {
            this.appSettings = appSettings.Value;
            this.mapper = mapper;
        }

        public async Task<UserOut> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            var user = await Task<User>.FromResult(_users.SingleOrDefault(x => x.Email == request.LoginDetails.Email));

            // return null if user not found
            if (user == null)
                return null;

            bool passMatchesHash =
                BCrypt.Net.BCrypt.Verify(request.LoginDetails.Password, user.PasswordHash);

            // return null when wrong password
            if (!passMatchesHash)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userOut = mapper.Map<UserOut>(user);
            userOut.Token = tokenHandler.WriteToken(token);

            return userOut;
        }
    }
}
