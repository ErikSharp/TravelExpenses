using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Locations
{
    public class UpdateLocation
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
            private readonly ILogger logger; 

            public Handler(
                TravelExpensesContext context,
                ILoggerFactory loggerFactory)
            {
                this.context = context;
                this.logger = loggerFactory.CreateLogger(nameof(Handler));
            }

            protected override async Task Handle(Command request, CancellationToken response)
            {
                var location = await context.Locations.Where(l => 
                    l.UserId == request.Location.UserId && 
                    l.Id == request.Location.Id)
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                if (location != null)
                {
                    location.LocationName = request.Location.LocationName;
                    location.CountryId = request.Location.CountryId;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    var msg = $"Location {request.Location.Id} not found for user {request.Location.UserId}";
                    throw new NotFoundException(msg);
                }
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
