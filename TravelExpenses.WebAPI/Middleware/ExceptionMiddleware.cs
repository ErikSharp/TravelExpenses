using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TravelExpenses.Application.Exceptions;
using TravelExpenses.WebAPI.Models;

namespace TravelExpenses.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
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
                _logger.LogInformation($"ValidationException: {ex.ToString()}");
                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    $"{ex.GetType().Name}: {ex.ToString()}");
            }
            catch (UserAlreadyExistsException ex)
            {
                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.Conflict,
                    ex.Message);
            }
            catch (SecurityException ex)
            {
                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (InfrastructureException ex)
            {
                await HandleExceptionAsync(
                    httpContext,
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex}");
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
