using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TravelExpenses.WebAPI.Middleware;
using TravelExpenses.WebAPI.Models;

namespace TravelExpenses.WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Got this from https://code-maze.com/global-error-handling-aspnetcore/
        /// </summary>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }

        /// <summary>
        /// This allows you to use a custom middleware for exceptions that is much
        /// easier to read than the one above
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
