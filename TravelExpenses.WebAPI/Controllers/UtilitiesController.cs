using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TravelExpenses.Application.Common.Dtos.Import;
using TravelExpenses.Application.Features.Utilities;
using TravelExpenses.Application.Helpers;
using TravelExpenses.Application.Interfaces;
using TravelExpenses.Domain.Entities;
using TravelExpenses.Persistence;
using TravelExpenses.WebAPI.Extensions;
using TravelExpenses.WebAPI.Utilities;

namespace TravelExpenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IBackgroundTaskQueue queue;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly INameGenerator nameGenerator;
        private readonly TravelExpensesContext context;
        private readonly AppSettings appSettings;

        public UtilitiesController(
            IOptions<AppSettings> appSettings,
            IBackgroundTaskQueue queue,
            IServiceScopeFactory serviceScopeFactory,
            INameGenerator nameGenerator,
            TravelExpensesContext context)
        {
            this.queue = queue;
            this.serviceScopeFactory = serviceScopeFactory;
            this.nameGenerator = nameGenerator;
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        [HttpPost("import-user/{userId}")]
        public IActionResult ImportUser(
            [FromBody]UserImportDto import,
            [FromHeader(Name = "Authorization")]string token,
            int userId)
        {
            var tokenUserId = User.Claims.GetUserId();

            if (!appSettings.AdminUserIds.Contains(tokenUserId))
            {
                return Forbid();
            }

            queue.QueueBackgroundWorkItem(async cancelToken =>
            {
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2#call-services-from-main
                using (var serviceScope = serviceScopeFactory.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var m = services.GetRequiredService<IMediator>();
                                        
                    Log.Information($"Starting ImportUser for user {userId}");
                    await m.Send(new ImportUser.Command(import, userId)).ConfigureAwait(false);
                }

                Log.Information("ImportUser has completed");
            });

            return Accepted();
        }

        [HttpPost("synthesize-user")]
        public IActionResult SynthesizeUser(
            [FromHeader(Name = "Authorization")]string token)
        {
            var tokenUserId = User.Claims.GetUserId();

            if (!appSettings.AdminUserIds.Contains(tokenUserId))
            {
                return Forbid();
            }

            string randomEmail;
            User existing;

            do
            {
                randomEmail = $"{nameGenerator.FirstName()}.{nameGenerator.Surname()}@testuser.com";
                existing = context.Users.SingleOrDefault(u => u.Email == randomEmail);

            } while (existing != null);

            queue.QueueBackgroundWorkItem(async cancelToken =>
            {
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2#call-services-from-main
                using (var serviceScope = serviceScopeFactory.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var m = services.GetRequiredService<IMediator>();

                    Log.Information($"Starting SynthesizeUser for");
                    await m.Send(new SynthesizeUser.Command(randomEmail)).ConfigureAwait(false);
                }

                Log.Information("SynthesizeUser has completed");
            });

            return Accepted(new { Email = randomEmail, Password = "123456" });
        }
    }
}
