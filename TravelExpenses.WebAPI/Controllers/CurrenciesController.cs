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

        public CurrenciesController(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await mediator.Send(new GetCurrencies.Query(memoryCache)).ConfigureAwait(false);
            return Ok(currencies);
        }
    }
}