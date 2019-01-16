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

namespace TravelExpenses.Application.Features.Currencies
{
    public class GetCurrencies
    {
        public class Query : IRequest<List<CurrencyOut>>
        {
        }

        public class Handler : IRequestHandler<Query, List<CurrencyOut>>
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

            public async Task<List<CurrencyOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currencies = await context.Currencies
                    .ToListAsync()
                    .ConfigureAwait(false);

                var currenciesOut = currencies.Select(c => mapper.Map<CurrencyOut>(c)).ToList();
                return currenciesOut;
            }
        }
    }
}
