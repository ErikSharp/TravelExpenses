using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Utilities
{
    public class GetBaseRequirements
    {
        public class Query : IRequest<BaseRequirements>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; private set; }
        }

        public class Handler : IRequestHandler<Query, BaseRequirements>
        {
            private readonly TravelExpensesContext context;
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory)
            {
                this.context = context;
                this.logger = loggerFactory.CreateLogger(nameof(Handler));
            }

            public async Task<BaseRequirements> Handle(Query request, CancellationToken cancellationToken)
            {
                var firstLocation = await context.Locations
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                var firstCategory = await context.Categories
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                var firstKeyword = await context.Keywords
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                var result = new BaseRequirements
                {
                    HasLocation = firstLocation != null,
                    HasCategory = firstCategory != null,
                    HasKeyword = firstKeyword != null
                };

                return result;
            }
        }
    }
}
