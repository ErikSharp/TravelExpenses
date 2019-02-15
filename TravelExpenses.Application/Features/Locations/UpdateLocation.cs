using AutoMapper;
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
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Locations
{
    public class UpdateLocation
    {
        public class Query : IRequest<LocationOut[]>
        {
            public Query(Location location)
            {
                Location = location;
            }

            public Location Location { get; }
        }

        public class Handler : IRequestHandler<Query, LocationOut[]>
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

            public async Task<LocationOut[]> Handle(Query request, CancellationToken response)
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

                var locations = await context.Locations
                    .Where(l => l.UserId == request.Location.UserId)
                    .Include(l => l.Country)
                    .Include(l => l.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return locations.Select(l => mapper.Map<LocationOut>(l)).ToArray();
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(c => c.Location.LocationName).NotEmpty().Length(3, 255);
            }
        }
    }
}
