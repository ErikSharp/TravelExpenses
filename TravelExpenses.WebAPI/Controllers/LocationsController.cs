
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Features.Locations;
using TravelExpenses.Domain.Entities;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator mediator;

        public LocationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var locations = await mediator.Send(new GetLocations.Query(userId)).ConfigureAwait(false);

            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]Location location)
        {
            var userId = User.Claims.GetUserId();
            location.UserId = userId;
            var locations = await mediator.Send(new CreateLocation.Query(location)).ConfigureAwait(false);

            return Created(
                new Uri(Request.Path, UriKind.Relative),
                locations);
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]Location location)
        {
            var userId = User.Claims.GetUserId();
            location.UserId = userId;
            await mediator.Send(new UpdateLocation.Command(location)).ConfigureAwait(false);

            return Ok();
        }
    }
}