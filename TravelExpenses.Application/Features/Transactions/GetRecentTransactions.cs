using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Transactions
{
    public class GetRecentTransactions
    {
        public class Query : IRequest<TransactionsOut>
        {
            public Query(int userId, int skip, int filterLocationId)
            {
                UserId = userId;
                Skip = skip;
                FilterLocationId = filterLocationId;
            }

            public int UserId { get; private set; }
            public int Skip { get; private set; }
            public int FilterLocationId { get; }
        }

        public class Handler : IRequestHandler<Query, TransactionsOut>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly AppSettings appSettings;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper,
                IOptions<AppSettings> appSettings)
            {
                this.context = context;
                this.mapper = mapper;
                this.appSettings = appSettings.Value;
            }

            public async Task<TransactionsOut> Handle(Query request, CancellationToken cancellationToken)
            {
                var totalQuery = context.Transactions
                    .Where(c => c.UserId == request.UserId);

                if (request.FilterLocationId > 0)
                {
                    totalQuery = totalQuery.Where(t => t.LocationId == request.FilterLocationId);
                }

                var totalRecords = await totalQuery.CountAsync().ConfigureAwait(false);

                var result = new TransactionsOut
                {
                    TotalRecords = totalRecords,
                    PageSize = appSettings.RecentTransactionsTakeAmount
                };

                if (totalRecords > 0)
                {
                    var query = context.Transactions
                        .Where(t => t.UserId == request.UserId);                        

                    if (request.FilterLocationId > 0)
                    {
                        query = query.Where(t => t.LocationId == request.FilterLocationId);
                    }

                    var page = await query
                        .Include(t => t.TransactionKeywords)
                        .OrderByDescending(t => t.TransDate)
                        .ThenByDescending(t => t.Id)
                        .Skip(request.Skip)
                        .Take(appSettings.RecentTransactionsTakeAmount)
                        .ToListAsync()
                        .ConfigureAwait(false);

                    var transactionsOut = page.Select(t => mapper.Map<TransactionOut>(t)).ToArray();
                    result.Transactions = transactionsOut;
                }

                return result;
            }
        }
    }
}
