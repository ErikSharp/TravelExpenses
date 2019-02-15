using AutoMapper;
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
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Locations
{
    public class GetLocations
    {
        public class Query : IRequest<List<LocationOut>>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; private set; }
        }

        public class Handler : IRequestHandler<Query, List<LocationOut>>
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

            public async Task<List<LocationOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var locations = await context.Locations
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .Include(l => l.Country)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var locationsOut = locations.Select(c => mapper.Map<LocationOut>(c)).ToList();
                return locationsOut;
            }
        }
    }
}
