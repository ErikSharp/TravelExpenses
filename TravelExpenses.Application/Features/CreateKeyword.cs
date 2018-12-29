using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features
{
    public class CreateKeyword
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

            protected override Task Handle(Command request, CancellationToken response)
            {
                context.Keywords.Add(request.Keyword);
                return context.SaveChangesAsync();
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
