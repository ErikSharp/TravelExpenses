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

namespace TravelExpenses.Application.Features.Transactions
{
    public class CreateTransaction
    {
        public static readonly string DateStringFormat = "yyyy-MM-dd";

        public class Command : IRequest
        {
            public Command(TransactionCreateIn transactionIn, int tokenUserId)
            {
                TransactionIn = transactionIn;
                TokenUserId = tokenUserId;
            }

            public TransactionCreateIn TransactionIn { get; }
            public int TokenUserId { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory,
                IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
                this.logger = loggerFactory.CreateLogger<CreateTransaction>();
            }

            protected override Task Handle(Command request, CancellationToken response)
            {
                var transaction = mapper.Map<Transaction>(request.TransactionIn);

                context.Transactions.Add(transaction);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.TransactionIn).NotNull().DependentRules(() =>
                {
                    RuleFor(c => c.TransactionIn.TransDate).Must(ParseAsDate);
                    RuleFor(c => c.TransactionIn.UserId).Equal(c => c.TokenUserId);
                    RuleFor(c => c.TransactionIn.LocationId).GreaterThan(0);
                    RuleFor(c => c.TransactionIn.CurrencyId).GreaterThan(0);
                    RuleFor(c => c.TransactionIn.CategoryId).GreaterThan(0);
                    RuleFor(c => c.TransactionIn.Title).NotEmpty().Length(2, 255);
                    RuleForEach(c => c.TransactionIn.KeywordIds).GreaterThan(0);                    
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
