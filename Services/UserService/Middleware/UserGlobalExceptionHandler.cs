using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Middleware
{
    public class UserGlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<UserGlobalExceptionHandler> _logger;

        public UserGlobalExceptionHandler(ILogger<UserGlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = CreateProblemDetails(exception);
            if (problemDetails == null)
                return false;

            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);


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
                //case LoginException ex:
                //    problemDetails.Status = StatusCodes.Status401Unauthorized;
                //    problemDetails.Title = "Unauthorized access";
                //    problemDetails.Detail = ex.Message;
                //    break;
                default:
                    return null;
            }

            return problemDetails;
        }
    }
}
