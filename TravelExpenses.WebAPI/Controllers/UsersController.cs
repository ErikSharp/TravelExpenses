using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Users;
using TravelExpenses.WebAPI.Models;

namespace TravelExpenses.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public const string InvalidCredsMsg = "Username or password is incorrect";
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPut("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserIn userParam)
        {
            if (string.IsNullOrEmpty(userParam.Password) || string.IsNullOrEmpty(userParam.Email))
            {
                return BadRequest(new ErrorDetails { Message = InvalidCredsMsg });
            }

            var authenticatedUser = await mediator.Send(new GetAuthenticatedUser.Query(userParam)).ConfigureAwait(false);

            if (authenticatedUser == null)
            {
                return BadRequest(new ErrorDetails { Message = InvalidCredsMsg });
            }

            return Ok(authenticatedUser);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserRegistration userParam)
        {
            var authenticatedUser = await mediator.Send(new CreateUser.Command(userParam));

            return Created(
                new Uri(Request.Path, UriKind.Relative), 
                authenticatedUser);
        }
    }
}