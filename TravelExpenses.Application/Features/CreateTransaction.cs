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

namespace TravelExpenses.Application.Features
{
    public class CreateTransaction
    {
        const string DateStringFormat = "yyyyMMdd";

        public class Command : IRequest
        {
            public Command(TransactionIn transactionIn, int tokenUserId)
            {
                TransactionIn = transactionIn;
                TokenUserId = tokenUserId;
            }

            public TransactionIn TransactionIn { get; }
            public int TokenUserId { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory)
            {
                this.context = context;
                this.logger = loggerFactory.CreateLogger<CreateTransaction>();
            }

            protected override async Task Handle(Command request, CancellationToken response)
            {
                var transDate = new SqlParameter
                {
                    ParameterName = "@TransDate",
                    SqlDbType = System.Data.SqlDbType.Date,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = DateTime.ParseExact(request.TransactionIn.TransDate, DateStringFormat, CultureInfo.InvariantCulture)
                };

                var amount = new SqlParameter
                {
                    ParameterName = "@Amount",
                    SqlDbType = System.Data.SqlDbType.SmallMoney,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.Amount
                };

                var locationId = new SqlParameter
                {
                    ParameterName = "@LocationId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.LocationId
                };

                var currencyId = new SqlParameter
                {
                    ParameterName = "@CurrencyId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.CurrencyId
                };

                var categoryId = new SqlParameter
                {
                    ParameterName = "@CategoryId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.CategoryId
                };

                var title = new SqlParameter
                {
                    ParameterName = "@Title",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 255,
                    Value = request.TransactionIn.Title
                };

                var memo = new SqlParameter
                {
                    ParameterName = "@Memo",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = -1, //this means max
                    Value = request.TransactionIn.Memo
                };

                var paidWithCash = new SqlParameter
                {
                    ParameterName = "@PaidWithCash",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.PaidWithCash
                };

                var userId = new SqlParameter
                {
                    ParameterName = "@UserId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = request.TransactionIn.UserId
                };

                try
                {
                    await context.Database.ExecuteSqlCommandAsync(
                        "EXEC [app].[usp_transaction_insert] @TransDate, @Amount, @LocationId, @CurrencyId, @CategoryId, @Title, @Memo, @PaidWithCash, @UserId",
                        transDate,
                        amount,
                        locationId,
                        currencyId,
                        categoryId,
                        title,
                        memo,
                        paidWithCash,
                        userId).ConfigureAwait(false);
                }
                catch (SqlException ex)
                {
                    var message = "An error occurred communicating with the database";
                    logger.LogError(ex, message);
                    throw new InfrastructureException(message);
                }
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
                    RuleFor(c => c.TransactionIn.Title).Length(3, 255);
                    RuleForEach(c => c.TransactionIn.KeywordIds).GreaterThan(0);                    
                });
            }

            private bool ParseAsDate(string dateString)
            {
                try
                {
                    DateTime.ParseExact(dateString, DateStringFormat, CultureInfo.InvariantCulture);                    
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
