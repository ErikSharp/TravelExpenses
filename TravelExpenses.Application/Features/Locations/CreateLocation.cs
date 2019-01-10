using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Locations
{
    public class CreateLocation
    {
        public class Command : IRequest
        {
            public Command(Location location)
            {
                Location = location;
            }

            public Location Location { get; }
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
                context.Locations.Add(request.Location);
                return context.SaveChangesAsync();
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Location.LocationName).NotEmpty().Length(3, 255);
            }
        }
    }
}
