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

namespace TravelExpenses.Application.Features.Transactions
{
    public class UpdateTransaction
    {
        public static readonly string DateStringFormat = "yyyy-MM-dd";

        public class Command : IRequest
        {
            public Command(TransactionEditIn transaction, int tokenUserId)
            {
                TransactionIn = transaction;
                TokenUserId = tokenUserId;
            }

            public TransactionEditIn TransactionIn { get; }
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
                if (request.TransactionIn.UserId != request.TokenUserId)
                {
                    throw new NotFoundException($"User {request.TokenUserId} is trying to update a transaction for user {request.TransactionIn.UserId}");
                }

                var dbTransaction = await context.Transactions
                    .Include(t => t.TransactionKeywords)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => 
                        t.Id == request.TransactionIn.Id)
                    .ConfigureAwait(false);

                if (dbTransaction == null)
                {
                    throw new NotFoundException($"Trying to update transaction {request.TransactionIn.Id} and it does not exist");
                }

                var transaction = mapper.Map<Transaction>(request.TransactionIn);
                var updateKeywords = transaction.TransactionKeywords
                    .Select(tk => tk.KeywordId)
                    .ToArray();

                var dbKeywords = dbTransaction.TransactionKeywords
                    .Select(tk => tk.KeywordId)
                    .ToArray();

                var keywordsToAdd = updateKeywords.Except(dbKeywords);
                var keywordsToDelete = dbKeywords.Except(updateKeywords);

                transaction.TransactionKeywords.Clear();

                if (keywordsToAdd.Any())
                {
                    foreach (var id in keywordsToAdd)
                    {
                        transaction.TransactionKeywords.Add(new TransactionKeyword
                        {
                            KeywordId = id
                        });
                    }
                }                

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync().ConfigureAwait(false);

                if (keywordsToDelete.Any())
                {
                    await context.Database.ExecuteSqlCommandAsync(
                        $"DELETE FROM [app].[TransactionKeyword] WHERE TransactionId = {transaction.Id} and KeywordId in ({string.Join(',', keywordsToDelete)})"
                    ).ConfigureAwait(false);
                }
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.TransactionIn).NotNull().DependentRules(() =>
                {
                    RuleFor(c => c.TransactionIn.Id).GreaterThan(0);
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
