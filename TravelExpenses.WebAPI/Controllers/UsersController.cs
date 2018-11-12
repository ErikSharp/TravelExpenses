using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Features;

namespace TravelExpenses.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]GetAuthenticatedUser.UserIn userParam)
        {
            var authenticatedUser = await mediator.Send(new GetAuthenticatedUser.Query(userParam));

            if (authenticatedUser == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(authenticatedUser);
        }
    }
}