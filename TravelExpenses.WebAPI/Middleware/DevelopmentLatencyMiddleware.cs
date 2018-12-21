using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TravelExpenses.WebAPI.Middleware
{
    public class DevelopmentLatencyMiddleware
    {
        private readonly RequestDelegate next;

        public DevelopmentLatencyMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await Task.Delay(2000);
            await next(httpContext);
        }
    }
}
