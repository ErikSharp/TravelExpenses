using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TravelExpenses.WebAPI.HealthChecks
{
    public static class EnvironmentHealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddEnvironmentCheck(
            this IHealthChecksBuilder builder)
        {
            builder.AddCheck<EnvironmentHealthCheck>("Environment");

            return builder;
        }
    }

    public class EnvironmentHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var name = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var data = new Dictionary<string, object>();
            data.Add("environment", name);

            return Task.FromResult(HealthCheckResult.Passed("The environment the server is running in", data));
        }
    }
}
