using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Exceptions;

namespace ReviewService.Middlewares
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = CreateProblemDetails(exception);

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private ProblemDetails CreateProblemDetails(Exception exception)
        {
            var problemDetails = new ProblemDetails();

            switch (exception)
            {
                case EntityNotFoundException enfex:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Not Found";
                    problemDetails.Detail = enfex.Message;
                    break;

                case UnauthorizedAccessException uaex:
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = uaex.Message;
                    break;

                case ArgumentNullException anex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Argument Null";
                    problemDetails.Detail = anex.Message;
                    break;

                case ArgumentException argex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = argex.Message;
                    break;

                case InvalidOperationException ioex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid Operation";
                    problemDetails.Detail = ioex.Message;
                    break;

                case FormatException fxex:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Format Error";
                    problemDetails.Detail = fxex.Message;
                    break;

                case NotImplementedException niex:
                    problemDetails.Status = StatusCodes.Status501NotImplemented;
                    problemDetails.Title = "Not Implemented";
                    problemDetails.Detail = niex.Message;
                    break;

                case TimeoutException texex:
                    problemDetails.Status = StatusCodes.Status504GatewayTimeout;
                    problemDetails.Title = "Timeout";
                    problemDetails.Detail = texex.Message;
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Server Error";
                    problemDetails.Detail = "An unexpected error occurred.";
                    break;
            }

            return problemDetails;
        }
    }
}
