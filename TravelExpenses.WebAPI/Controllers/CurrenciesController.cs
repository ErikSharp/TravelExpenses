using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Currencies;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;
        private readonly string cacheKey = Guid.NewGuid().ToString();

        public CurrenciesController(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cachedCurrencies = memoryCache.Get<List<CurrencyOut>>(cacheKey);
            if (cachedCurrencies != null)
                return Ok(cachedCurrencies);

            var currencies = await mediator.Send(new GetCurrencies.Query()).ConfigureAwait(false);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            memoryCache.Set(cacheKey, currencies, cacheEntryOptions);

            return Ok(currencies);
        }
    }
}