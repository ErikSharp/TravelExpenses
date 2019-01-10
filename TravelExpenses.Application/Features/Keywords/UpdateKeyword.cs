using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Keywords
{
    public class UpdateKeyword
    {
        public class Command : IRequest
        {
            public Command(Keyword keyword)
            {
                Keyword = keyword;
            }

            public Keyword Keyword { get; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly TravelExpensesContext context;

            public Handler(
                TravelExpensesContext context)
            {
                this.context = context;
            }

            protected async override Task Handle(Command request, CancellationToken response)
            {
                var keyword = await context.Keywords.Where(k =>
                    k.UserId == request.Keyword.UserId &&
                    k.Id == request.Keyword.Id)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (keyword != null)
                {
                    keyword.KeywordName = request.Keyword.KeywordName;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    var msg = $"Keyword {request.Keyword.Id} not found for user {request.Keyword.UserId}";
                    throw new NotFoundException(msg);
                }
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Keyword.KeywordName).NotEmpty().Length(3, 255);
            }
        }
    }
}
