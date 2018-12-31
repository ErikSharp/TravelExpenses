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

namespace TravelExpenses.Application.Features.Categories
{
    public class GetCategories
    {
        public class Query : IRequest<List<CategoryOut>>
        {
            public Query(int userId)
            {
                UserId = userId;
            }

            public int UserId { get; private set; }
        }

        public class Handler : IRequestHandler<Query, List<CategoryOut>>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private readonly ILogger logger;

            public Handler(
                TravelExpensesContext context,
                IMapper mapper,
                ILoggerFactory loggerFactory)
            {
                this.context = context;
                this.mapper = mapper;
                this.logger = loggerFactory.CreateLogger(nameof(Handler));
            }

            public async Task<List<CategoryOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await context.Categories
                    .Where(c => c.UserId == request.UserId)
                    .Include(c => c.User)
                    .ToListAsync()
                    .ConfigureAwait(false);

                var categoriesOut = categories.Select(c => mapper.Map<CategoryOut>(c)).ToList();
                return categoriesOut;
            }
        }
    }
}
