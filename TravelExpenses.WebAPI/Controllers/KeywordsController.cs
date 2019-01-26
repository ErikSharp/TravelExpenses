
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
        private object k;

        public KeywordsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            var countries = await mediator.Send(new GetKeywords.Query(userId)).ConfigureAwait(false);

            return Ok(countries);
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]string[] keywordNames)
        {
            var userId = User.Claims.GetUserId();
            var keywords = keywordNames.Select(k => new Keyword() { KeywordName = k, UserId = userId }).ToArray();
            var keywordStrings = await mediator.Send(new CreateKeyword.Query(keywords)).ConfigureAwait(false);

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