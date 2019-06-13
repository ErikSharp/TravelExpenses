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
            public Query(int currencyId, int userId)
            {
                CurrencyId = currencyId;
                UserId = userId;
            }

            public int CurrencyId { get; }
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
                var totalSpent = context.Transactions
                    .Include(t => t.Category)
                    .Where(t => 
                        t.UserId == query.UserId && 
                        t.CurrencyId == query.CurrencyId &&
                        t.Category.CategoryName.ToUpperInvariant() != "Loss/Gain".ToUpperInvariant() &&
                        t.PaidWithCash)
                    .Sum(t => t.Amount);

                var totalWithdrawn = context.CashWithdrawals
                    .Where(t =>
                        t.UserId == query.UserId &&
                        t.CurrencyId == query.CurrencyId)
                    .Sum(t => t.Amount);

                var totalLossGain = context.Transactions
                    .Include(t => t.Category)
                    .Where(t =>
                        t.UserId == query.UserId &&
                        t.CurrencyId == query.CurrencyId &&
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
                                t.CurrencyId == query.CurrencyId)
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
