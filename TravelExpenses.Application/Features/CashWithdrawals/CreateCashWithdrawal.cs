using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.CashWithdrawals
{
    public class CreateCashWithdrawal
    {
        public static readonly string DateStringFormat = "yyyy-MM-dd";

        public class Command : IRequest
        {
            public Command(CashWithdrawalCreateIn cashWithdrawalIn, int tokenUserId)
            {
                CashWithdrawalIn = cashWithdrawalIn;
                TokenUserId = tokenUserId;
            }

            public CashWithdrawalCreateIn CashWithdrawalIn { get; }
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
                var cashWithdrawal = mapper.Map<CashWithdrawal>(request.CashWithdrawalIn);

                context.CashWithdrawals.Add(cashWithdrawal);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.CashWithdrawalIn).NotNull().DependentRules(() =>
                {
                    RuleFor(c => c.CashWithdrawalIn.Title).NotEmpty().Length(3, 255);
                    RuleFor(c => c.CashWithdrawalIn.Amount).GreaterThan(0m);
                    RuleFor(c => c.CashWithdrawalIn.TransDate).Must(ParseAsDate);
                    RuleFor(c => c.CashWithdrawalIn.CurrencyId).GreaterThan(0);
                    RuleFor(c => c.CashWithdrawalIn.LocationId).GreaterThan(0);
                    RuleFor(c => c.CashWithdrawalIn.UserId).Equal(c => c.TokenUserId);
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
