using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;

namespace TravelExpenses.WebAPI.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            try
            {
                var userIdClaim = claims.Single(c => c.Type == "userId");

                return int.Parse(userIdClaim.Value);
            }
            catch (Exception)
            {
                throw new SecurityException("There is a problem with the token");
            }
        }
    }
}
