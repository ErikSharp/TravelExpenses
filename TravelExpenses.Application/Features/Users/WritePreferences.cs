using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Users
{
    public class WritePreferences
    {
        public class Query : IRequest<UserOut>
        {
            public Query(int userId, Preferences preferences)
            {
                UserId = userId;
                Preferences = preferences;
            }

            public int UserId { get; }
            public Preferences Preferences { get; }
        }

        public class Handler : IRequestHandler<Query, UserOut>
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

            public async Task<UserOut> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await context.Users
                    .SingleOrDefaultAsync(u => u.Id == request.UserId)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    throw new NotFoundException($"User {request.UserId} not found");
                }

                var json = JsonConvert.SerializeObject(request.Preferences);
                user.Preferences = json;
                await context.SaveChangesAsync().ConfigureAwait(false);

                return mapper.Map<UserOut>(user);
            }
        }
    }
}
