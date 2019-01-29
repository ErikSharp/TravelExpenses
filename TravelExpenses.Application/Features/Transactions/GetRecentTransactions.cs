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
        public class Query : IRequest<TransactionOut[]>
        {
            public Query(int userId, int skip)
            {
                UserId = userId;
                Skip = skip;
            }

            public int UserId { get; private set; }
            public int Skip { get; private set; }
        }

        public class Handler : IRequestHandler<Query, TransactionOut[]>
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

            public async Task<TransactionOut[]> Handle(Query request, CancellationToken cancellationToken)
            {
                var transactions = await context.Transactions
                    .Where(t => t.UserId == request.UserId)
                    .Include(t => t.TransactionKeywords)
                    .OrderByDescending(t => t.TransDate)
                    .ThenByDescending(t => t.Id)
                    .Take(appSettings.RecentTransactionsTakeAmount)
                    .Skip(request.Skip)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var transactionsOut = transactions.Select(t => mapper.Map<TransactionOut>(t)).ToArray();
                return transactionsOut;
            }
        }
    }
}
