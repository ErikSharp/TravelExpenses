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
            public Query(int userId, int skip)
            {
                UserId = userId;
                Skip = skip;
            }

            public int UserId { get; private set; }
            public int Skip { get; private set; }
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
                var totalRecords = context.Transactions
                    .Count(c => c.UserId == request.UserId);

                var result = new TransactionsOut
                {
                    TotalRecords = totalRecords,
                    PageCount = appSettings.RecentTransactionsTakeAmount
                };

                if (totalRecords > 0)
                {
                    var page = await context.Transactions
                        .Where(t => t.UserId == request.UserId)
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
