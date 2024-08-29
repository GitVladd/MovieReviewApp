using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserService.Exceptions;

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
                case PasswordRequiresDigitException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password requires digit";
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordRequiresLowercaseException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password requires lowercase letter";
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordRequiresUppercaseException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password requires uppercase letter";
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordRequiresNonAlphanumericException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password requires non-alphanumeric character";
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordTooShortException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password too short";
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordRequiresUniqueCharsException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Password requires unique characters";
                    problemDetails.Detail = exception.Message;
                    break;
                case UsernameMustBeUniqueException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Username must be unique";
                    problemDetails.Detail = exception.Message;
                    break;
                case UserRequiresUniqueEmailException _:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Email must be unique";
                    problemDetails.Detail = exception.Message;
                    break;

                default:
                    return null;
            }

            return problemDetails;
        }
    }
}
