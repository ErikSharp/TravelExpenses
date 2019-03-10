
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpenses.Application.Features.Keywords;
using TravelExpenses.Domain.Entities;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly IMediator mediator;

        public KeywordsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var keywords = await mediator.Send(new GetKeywords.Query(userId)).ConfigureAwait(false);

            return Ok(keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]string[] keywordNames)
        {
            var userId = User.Claims.GetUserId();
            
            var keywordStrings = await mediator.Send(new CreateKeyword.Query(keywordNames, userId)).ConfigureAwait(false);

            return Created(
                new Uri(Request.Path, UriKind.Relative),
                keywordStrings);
        }

        [HttpPut]
        public async Task<IActionResult> Put(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]Keyword keyword)
        {
            var userId = User.Claims.GetUserId();
            keyword.UserId = userId;
            var keywordStrings = await mediator.Send(new UpdateKeyword.Query(keyword)).ConfigureAwait(false);

            return Ok(keywordStrings);
        }
    }
}