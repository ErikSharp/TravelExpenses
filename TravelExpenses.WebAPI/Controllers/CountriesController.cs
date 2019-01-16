
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Countries;
using TravelExpenses.Domain.Entities;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;
        private readonly string cacheKey = Guid.NewGuid().ToString();

        public CountriesController(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cachedCountries = memoryCache.Get<List<CountryOut>>(cacheKey);
            if (cachedCountries != null)
                return Ok(cachedCountries);

            var countries = await mediator.Send(new GetCountries.Query()).ConfigureAwait(false);

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            memoryCache.Set(cacheKey, countries, cacheEntryOptions);

            return Ok(countries);
        }
    }
}