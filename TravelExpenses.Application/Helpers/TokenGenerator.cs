using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Common;
using TravelExpenses.Domain.Entities;

namespace TravelExpenses.Application.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IDateTime dateTime;
        private AppSettings appSettings;

        public TokenGenerator(
            IOptions<AppSettings> appSettings,
            IDateTime dateTime)
        {
            this.appSettings = appSettings.Value;
            this.dateTime = dateTime;
        }

        public string CreateTokenString(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = dateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
