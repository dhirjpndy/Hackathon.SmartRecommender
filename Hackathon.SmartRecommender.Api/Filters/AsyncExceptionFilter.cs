using Hackathon.SmartRecommender.Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.SmartRecommender.Api.Filters
{
    /// <summary>
    /// A global exception filter that will handle all unhandled exceptions for the application.
    /// </summary>
    internal class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<AsyncExceptionFilter> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public AsyncExceptionFilter(ILogger<AsyncExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ServiceException serviceException)
            {
                context.Result = GetResult(serviceException);
            }
            else
            {
                context.Result = GetResult(context.Exception);
            }

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        private IActionResult GetResult(Exception exception)
        {
            var logReferenceId = Guid.NewGuid().ToString();

            // this will generate: "{ExceptionType} - {Message}." or "{ExceptionType}." if no message is present
            static string GetExceptionDescription(Exception exception)
            {
                var s = exception.GetType().ToString();

                if (!string.IsNullOrEmpty(exception.Message))
                {
                    s += $" - {exception.Message}";
                }

                return $"{s.TrimEnd('.')}."; // ensure there's a period at the end
            }

            _logger.LogError(exception, $"Unhandled Exception: {GetExceptionDescription(exception)} ReferenceId: {logReferenceId}");

            var content = $"An unexpected error has occurred.  You can use the following reference id to help us diagnose your problem: '{logReferenceId}'";

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(content),
                StatusCode = StatusCodes.Status500InternalServerError,
                ContentType = "application/json"
            };
        }

        private IActionResult GetResult(ServiceException exception)
        {
            var responseBody = new ProblemDetails
            {
                Type = exception.ErrorCode.ToString(),
                Title = exception.ErrorCode.GetDescription(),
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Detail
            };

            if (_hostEnvironment.IsDevelopment())
            {
                responseBody.Extensions.Add("stackTrace", exception.ToString());
            }

            _logger.LogInformation(exception, $"Service Exception: {exception.Message}");

            return new BadRequestObjectResult(responseBody)
            {
                ContentTypes = { "application/problem+json", "application/problem+xml" }
            };
        }
    }
}
