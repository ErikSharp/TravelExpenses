using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Users;
using TravelExpenses.Application.Features.Utilities;
using TravelExpenses.WebAPI.Extensions;
using TravelExpenses.WebAPI.Models;

namespace TravelExpenses.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IMediator mediator;

        public UtilitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("base-requirements")]
        public async Task<IActionResult> GetBaseRequirements(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var baseRequirements = await mediator.Send(new GetBaseRequirements.Query(userId)).ConfigureAwait(false);

            return Ok(baseRequirements);
        }
    }
}