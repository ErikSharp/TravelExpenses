using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Categories;
using TravelExpenses.Application.Features.Countries;
using TravelExpenses.Application.Features.Currencies;
using TravelExpenses.Application.Features.Keywords;
using TravelExpenses.Application.Features.Locations;
using TravelExpenses.Application.Features.Users;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/base-data")]
    [ApiController]
    public class BaseDataController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;

        public BaseDataController(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Claims.GetUserId();
            var locations = await mediator.Send(new GetLocations.Query(userId)).ConfigureAwait(false);
            var keywords = await mediator.Send(new GetKeywords.Query(userId)).ConfigureAwait(false);
            var categories = await mediator.Send(new GetCategories.Query(userId)).ConfigureAwait(false);
            var user = await mediator.Send(new GetThisUser.Query(userId)).ConfigureAwait(false);
            var countries = await mediator.Send(new GetCountries.Query(memoryCache)).ConfigureAwait(false);
            var currencies = await mediator.Send(new GetCurrencies.Query(memoryCache)).ConfigureAwait(false);

            return Ok(new
            {
                locations,
                keywords,
                categories,
                user,
                countries,
                currencies
            });
        }
    }
}
