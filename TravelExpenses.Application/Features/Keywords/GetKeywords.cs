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

namespace TravelExpenses.Application.Features.Keywords
{
    public class GetKeywords
    {
        public class Query : IRequest<List<KeywordOut>>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; private set; }
        }

        public class Handler : IRequestHandler<Query, List<KeywordOut>>
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

            public async Task<List<KeywordOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var keywords = await context.Keywords
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var keywordsOut = keywords.Select(k => mapper.Map<KeywordOut>(k)).ToList();
                return keywordsOut;
            }
        }
    }
}
