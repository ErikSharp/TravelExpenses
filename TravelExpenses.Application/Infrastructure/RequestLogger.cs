using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravelExpenses.Application.Infrastructure
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;

        public RequestLogger(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            // TODO: Add User Details
            
            _logger.LogInformation(
                "TravelExpenses Request: {Name} {@Request}",
                typeof(TRequest).DeclaringType.Name, 
                request);

            return Task.CompletedTask;
        }
    }
}
