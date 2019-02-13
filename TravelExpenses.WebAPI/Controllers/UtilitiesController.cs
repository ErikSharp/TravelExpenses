using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos.Import;
using TravelExpenses.Application.Features.Transactions;
using TravelExpenses.Application.Helpers;
using TravelExpenses.WebAPI.Extensions;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly AppSettings appSettings;

        public UtilitiesController(
            IMediator mediator,
            IOptions<AppSettings> appSettings)
        {
            this.mediator = mediator;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("import-user/{userId}")]
        public async Task<IActionResult> ImportUser(
            [FromBody]UserImportDto import,
            [FromHeader(Name = "Authorization")]string token,
            int userId)
        {
            var tokenUserId = User.Claims.GetUserId();

            if (tokenUserId != appSettings.ErikUserId)
            {
                return Forbid();
            }

            await mediator.Send(new ImportUser.Command(import, userId)).ConfigureAwait(false);

            return Created(
                new Uri(Request.Path, UriKind.Relative),
                null);
        }
    }
}
