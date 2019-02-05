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
    public class DeleteTransaction
    {
        public class Command : IRequest
        {
            public Command(int transactionId, int tokenUserId)
            {
                TransactionId = transactionId;
                TokenUserId = tokenUserId;
            }

            public int TransactionId { get; }
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

            protected override async Task Handle(Command request, CancellationToken response)
            {
                var transaction = await context.Transactions
                    .Include(t => t.User)
                    .Include(t => t.TransactionKeywords)
                    .SingleOrDefaultAsync(t => t.Id == request.TransactionId && t.UserId == request.TokenUserId)
                    .ConfigureAwait(false);

                if (transaction != null)
                {                    
                    context.Transactions.Remove(transaction);

                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException($"Transaction {request.TransactionId} not found");
                }
            }
        }
    }
}
