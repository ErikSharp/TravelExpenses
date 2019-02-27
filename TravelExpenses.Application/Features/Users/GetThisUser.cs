using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;

namespace TravelExpenses.Application.Features.Users
{
    public class GetThisUser
    {
        public class Query : IRequest<UserOut>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; }
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

                return mapper.Map<UserOut>(user);
            }
        }
    }
}
