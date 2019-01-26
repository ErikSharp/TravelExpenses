using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Keywords
{
    public class CreateKeyword
    {
        public class Command : IRequest
        {
            public Command(Keyword[] keywords)
            {
                Keywords = keywords;
            }

            public Keyword[] Keywords { get; }
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
                context.Keywords.AddRange(request.Keywords);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleForEach(c => c.Keywords).SetValidator(new KeywordValidator());
            }

            public class KeywordValidator : AbstractValidator<Keyword>
            {
                public KeywordValidator()
                {
                    RuleFor(c => c.KeywordName).NotEmpty().Length(3, 255);
                }
            }
        }
    }
}
