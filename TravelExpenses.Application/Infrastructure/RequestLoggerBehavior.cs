using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravelExpenses.Application.Infrastructure
{
    public class RequestLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Log.Information(
                "TravelExpenses Request: {Name} {@Request}",
                typeof(TRequest).DeclaringType.Name,
                request);

            return next();
        }
    }
}
