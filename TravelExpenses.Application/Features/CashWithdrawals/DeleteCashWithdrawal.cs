using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.CashWithdrawals
{
    public class DeleteCashWithdrawal
    {
        public class Command : IRequest
        {
            public Command(int cashWithdrawalId, int tokenUserId)
            {
                CashWithdrawalId = cashWithdrawalId;
                TokenUserId = tokenUserId;
            }

            public int CashWithdrawalId { get; }
            public int TokenUserId { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            protected override async Task Handle(Command request, CancellationToken response)
            {
                var cashWithdrawal = await context.CashWithdrawals
                    .Include(t => t.User)
                    .SingleOrDefaultAsync(t => t.Id == request.CashWithdrawalId && t.UserId == request.TokenUserId)
                    .ConfigureAwait(false);

                if (cashWithdrawal != null)
                {
                    context.CashWithdrawals.Remove(cashWithdrawal);

                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException($"Cash Withdrawal {request.CashWithdrawalId} not found");
                }
            }
        }
    }
}
