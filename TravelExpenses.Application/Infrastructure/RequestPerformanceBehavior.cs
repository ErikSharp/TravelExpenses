using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravelExpenses.Application.Infrastructure
{
    public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly int thresholdMs;
        public RequestPerformanceBehavior(IConfiguration configuration)
        {
            var configThreshold = configuration.GetValue<int>("AppSettings:LongRunningThresholdMs");
            if (configThreshold == 0)
            {
                Log.Warning("Could not get configuration from AppSettings:LongRunningThresholdMs so defaulting to 500ms");
                configThreshold = 500;
            }

            thresholdMs = configThreshold;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            if (timer.ElapsedMilliseconds > thresholdMs)
            {
                var name = typeof(TRequest).DeclaringType.Name;

                Log.Warning("TravelExpenses Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds)", name, timer.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
