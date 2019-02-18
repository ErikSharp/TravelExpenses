using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos;
using TravelExpenses.Application.Features.Reconcile;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReconcileController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReconcileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("currency-totals")]
        public async Task<IActionResult> GetCurrencyTotals(
            [FromHeader(Name = "Authorization")]string token,
            [FromBody]CurrencyTotalsRequest request)
        {
            var userId = User.Claims.GetUserId();
            var totals = await mediator.Send(new GetCurrencyTotals.Query(request, userId)).ConfigureAwait(false);

            return Ok(totals);
        }
    }
}
