using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
            public Query(IMemoryCache memoryCache = null)
            {
                MemoryCache = memoryCache;
            }

            public IMemoryCache MemoryCache { get; }
        }

        public class Handler : IRequestHandler<Query, List<CurrencyOut>>
        {
            private readonly TravelExpensesContext context;
            private readonly IMapper mapper;
            private static readonly string cacheKey = Guid.NewGuid().ToString();

            public Handler(
                TravelExpensesContext context,
                IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<List<CurrencyOut>> Handle(Query request, CancellationToken cancellationToken)
            {
                var haveMemoryCache = request.MemoryCache != null;
                if (haveMemoryCache)
                {
                    var cachedCurrencies = request.MemoryCache.Get<List<CurrencyOut>>(cacheKey);
                    if (cachedCurrencies != null)
                        return cachedCurrencies;
                }

                var currencies = await context.Currencies
                    .ToListAsync()
                    .ConfigureAwait(false);

                var currenciesOut = currencies.Select(c => mapper.Map<CurrencyOut>(c)).ToList();

                if (haveMemoryCache)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    request.MemoryCache.Set(cacheKey, currenciesOut, cacheEntryOptions);
                }

                return currenciesOut;
            }
        }
    }
}
