using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.CashWithdrawals
{
    public class UpdateCashWithdrawal
    {
        public static readonly string DateStringFormat = "yyyy-MM-dd";

        public class Command : IRequest
        {
            public Command(CashWithdrawalDto cashWithdrawal, int tokenUserId)
            {
                CashWithdrawal = cashWithdrawal;
                TokenUserId = tokenUserId;
            }

            public CashWithdrawalDto CashWithdrawal { get; }
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

            protected override Task Handle(Command request, CancellationToken response)
            {
                if (request.CashWithdrawal.UserId != request.TokenUserId)
                {
                    throw new NotFoundException($"User {request.TokenUserId} is trying to update a cash withdrawal for user {request.CashWithdrawal.UserId}");
                }

                var cashWithdrawal = mapper.Map<CashWithdrawal>(request.CashWithdrawal);

                context.CashWithdrawals.Update(cashWithdrawal);

                return context.SaveChangesAsync();                
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CashWithdrawal).NotNull().DependentRules(() =>
                {
                    RuleFor(c => c.CashWithdrawal.Id).GreaterThan(0);
                    RuleFor(c => c.CashWithdrawal.Title).NotEmpty().Length(3, 255);
                    RuleFor(c => c.CashWithdrawal.Amount).GreaterThan(0m);
                    RuleFor(c => c.CashWithdrawal.TransDate).Must(ParseAsDate);
                    RuleFor(c => c.CashWithdrawal.CurrencyId).GreaterThan(0);
                    RuleFor(c => c.CashWithdrawal.UserId).Equal(c => c.TokenUserId);
                });
            }

            private bool ParseAsDate(string dateString)
            {
                return DateTime.TryParseExact(
                    dateString,
                    DateStringFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var result);
            }
        }
    }
}
