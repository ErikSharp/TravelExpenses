using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Reconcile
{
    public class GetCurrencyTotals
    {
        public class Query : IRequest<CurrencyTotals>
        {
            public Query(CurrencyTotalsRequest request, int userId)
            {
                Request = request;
                UserId = userId;
            }

            public CurrencyTotalsRequest Request { get; }
            public int UserId { get; }
        }

        public class Handler : IRequestHandler<Query, CurrencyTotals>
        {
            private readonly TravelExpensesContext context;

            public Handler(TravelExpensesContext context)
            {
                this.context = context;
            }

            public Task<CurrencyTotals> Handle(Query query, CancellationToken cancellationToken)
            {
                var request = query.Request;

                var totalSpent = context.Transactions
                    .Where(t => 
                        t.UserId == query.UserId && 
                        t.LocationId == request.LocationId &&
                        t.CurrencyId == request.CurrencyId &&
                        t.PaidWithCash)
                    .Sum(t => t.Amount);

                var totalWithdrawn = context.CashWithdrawals
                    .Where(t =>
                        t.UserId == query.UserId &&
                        t.LocationId == request.LocationId &&
                        t.CurrencyId == request.CurrencyId)
                    .Sum(t => t.Amount);

                return Task.FromResult(new CurrencyTotals
                {
                    TotalSpent = totalSpent,
                    TotalWithdrawn = totalWithdrawn
                });
            }
        }
    }
}
