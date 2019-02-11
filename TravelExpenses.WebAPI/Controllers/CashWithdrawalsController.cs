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
    [Route("api/[controller]")]
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

        //[HttpPut]
        //public async Task<IActionResult> EditTransaction(
        //    [FromBody]TransactionEditIn transaction,
        //    [FromHeader(Name = "Authorization")]string token)
        //{
        //    var userId = User.Claims.GetUserId();
        //    await mediator.Send(new UpdateTransaction.Command(transaction, userId)).ConfigureAwait(false);

        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetRecent(
        //    [FromQuery(Name = "skip")] int skip,
        //    [FromHeader(Name = "Authorization")] string token)
        //{
        //    var userId = User.Claims.GetUserId();
        //    var transactions = await mediator.Send(new GetRecentTransactions.Query(userId, skip)).ConfigureAwait(false);

        //    return Ok(transactions);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTransaction(int id)
        //{
        //    var userId = User.Claims.GetUserId();
        //    await mediator.Send(new DeleteTransaction.Command(id, userId)).ConfigureAwait(false);

        //    return Ok();
        //}
    }
}
