using HetsApi.Extensions;
using HetsApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HetsApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted || httpContext.RequestAborted.IsCancellationRequested)
                    return;

                var guid = Guid.NewGuid();
                _logger.LogError($"HETS Exception {guid}: {ex}");
                await HandleExceptionAsync(httpContext, guid);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Guid guid)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new HetsResponse("Internal Server Error", "An unexpected error occurred!");

            //var problem = new ValidationProblemDetails()
            //{
            //    Type = "https://hets.bc.gov.ca/exception",
            //    Title = "An unexpected error occurred!",
            //    Status = StatusCodes.Status500InternalServerError,
            //    Detail = "The instance value should be used to identify the problem when calling customer support",
            //    Instance = $"urn:hets:error:{Guid.NewGuid()}"
            //};

            //problem.Extensions.Add("traceId", context.TraceIdentifier);

            await context.Response.WriteJsonAsync(problem, "application/problem+json");
        }
    }
}
