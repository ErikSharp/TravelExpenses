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
    public class CreateCountry
    {
        public class Command : IRequest
        {
            public Command(Country country)
            {
                Country = country;
            }

            public Country Country { get; }
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
                context.Countries.Add(request.Country);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Country.CountryName).NotEmpty().Length(3, 255);
            }
        }
    }
}
