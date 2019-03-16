using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.CashWithdrawals;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/cash-withdrawals")]
    [ApiController]
    public class CashWithdrawalsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CashWithdrawalsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCashWithdrawal(
            [FromBody]CashWithdrawalCreateIn cashWithdrawal,
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            await mediator.Send(new CreateCashWithdrawal.Command(cashWithdrawal, userId)).ConfigureAwait(false);

            return Created(
                new Uri(Request.Path, UriKind.Relative),
                null);
        }

        [HttpPut]
        public async Task<IActionResult> EditCashWithdrawal(
            [FromBody]CashWithdrawalDto cashWithdrawal,
            [FromHeader(Name = "Authorization")]string token)
        {
            var userId = User.Claims.GetUserId();
            await mediator.Send(new UpdateCashWithdrawal.Command(cashWithdrawal, userId)).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRecent(
            [FromQuery(Name = "skip")] int skip,
            [FromHeader(Name = "Authorization")] string token)
        {
            var userId = User.Claims.GetUserId();
            var result = await mediator.Send(new GetCashWithdrawals.Query(userId, skip)).ConfigureAwait(false);

            Response.Headers.Add("X-Total-Count", result.TotalRecords.ToString());
            Response.Headers.Add("Page-Count", result.PageCount.ToString());

            return Ok(result.CashWithdrawals);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashWithdrawal(int id)
        {
            var userId = User.Claims.GetUserId();
            await mediator.Send(new DeleteCashWithdrawal.Command(id, userId)).ConfigureAwait(false);

            return Ok();
        }
    }
}
