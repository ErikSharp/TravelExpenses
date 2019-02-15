using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.WebAPI.Models;

namespace TravelExpenses.WebAPI.Middleware
{
    /// <summary>
    /// Translates exceptions into API responses
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                Log.Warning(ex, "ValidationException");
                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    $"{ex.GetType().Name}: {ex.ToString()}");
            }
            catch (UserAlreadyExistsException ex)
            {
                Log.Warning(ex, ex.GetType().Name);

                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.Conflict,
                    ex.Message);
            }
            catch (SecurityException ex)
            {
                Log.Warning(ex, ex.GetType().Name);

                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (NotFoundException ex)
            {
                Log.Warning(ex, ex.GetType().Name);

                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.NotFound,
                    "The record was not found");
            }
            catch (DbUpdateException ex)
            {
                var e = ex.InnerException ?? ex;
                Log.Warning(e, ex.GetType().Name);

                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    "The database has rejected the request");
            }
            catch (InfrastructureException ex)
            {
                Log.Error(ex, ex.GetType().Name);

                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.GetType().Name);
                await HandleExceptionAsync(
                    httpContext, 
                    HttpStatusCode.InternalServerError, 
                    $"{ex.GetType().Name}: {ex.Message}");
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context, 
            HttpStatusCode httpStatusCode, 
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                Message = message
            }.ToString());
        }
    }
}
