using MediatR;
using Microsoft.EntityFrameworkCore;
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
        public static readonly string DateStringFormat = "yyyy-MM-dd";

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
                    .Include(t => t.Category)
                    .Where(t => 
                        t.UserId == query.UserId && 
                        t.LocationId == request.LocationId &&
                        t.CurrencyId == request.CurrencyId &&
                        t.Category.CategoryName.ToUpperInvariant() != "Loss/Gain".ToUpperInvariant() &&
                        t.PaidWithCash)
                    .Sum(t => t.Amount);

                var totalWithdrawn = context.CashWithdrawals
                    .Where(t =>
                        t.UserId == query.UserId &&
                        t.LocationId == request.LocationId &&
                        t.CurrencyId == request.CurrencyId)
                    .Sum(t => t.Amount);

                var totalLossGain = context.Transactions
                    .Include(t => t.Category)
                    .Where(t =>
                        t.UserId == query.UserId &&
                        t.LocationId == request.LocationId &&
                        t.CurrencyId == request.CurrencyId &&
                        t.Category.CategoryName.ToUpperInvariant() == "Loss/Gain".ToUpperInvariant() &&
                        t.PaidWithCash)
                    .Sum(t => t.Amount);

                string lastTransactionDay = string.Empty;

                try
                {
                    if (totalWithdrawn != 0)
                    {
                        lastTransactionDay = context.Transactions
                            .Where(t =>
                                t.UserId == query.UserId &&
                                t.LocationId == request.LocationId &&
                                t.CurrencyId == request.CurrencyId)
                            .Max(t => t.TransDate).ToString(DateStringFormat);
                    }
                }
                catch (InvalidOperationException)
                {
                }

                return Task.FromResult(new CurrencyTotals
                {
                    TotalSpent = totalSpent,
                    TotalWithdrawn = totalWithdrawn,
                    TotalLossGain = totalLossGain,
                    LastTransactionDay = lastTransactionDay
                });
            }
        }
    }
}
