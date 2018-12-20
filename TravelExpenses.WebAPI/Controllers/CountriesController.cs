﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Features;
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

        public CountriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var countries = await mediator.Send(new GetCountries.Query(userId)).ConfigureAwait(false);

            return Ok(countries);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]Country country)
        {
            var userId = User.Claims.GetUserId();
            country.UserId = userId;
            await mediator.Send(new CreateCountry.Command(country)).ConfigureAwait(false);
            
            return Created(
                new Uri(Request.Path, UriKind.Relative),
                null);
        }
    }
}