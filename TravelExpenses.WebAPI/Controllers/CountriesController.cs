
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

        public CountriesController(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await mediator.Send(new GetCountries.Query(memoryCache)).ConfigureAwait(false);
            return Ok(countries);
        }
    }
}