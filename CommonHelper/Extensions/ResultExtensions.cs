using MeesageService.Shared.Behaviour;
using MeesageService.Shared.Enums;
using Microsoft.AspNetCore.Diagnostics;

namespace MeesageService.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Result indicates success, but method expected a failure.");
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error.Type),
                title: GetTitle(result.Error.Type),
                type: GetErrorDetailUrl(result.Error.Type),
                extensions: new Dictionary<string, object?> {
                {
                        "errors", new[] { result.Error } }
                });
        }

        static int GetStatusCode(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }
        static string GetTitle(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Server Failure"
            };
        }
        static string GetErrorDetailUrl(ErrorType type)
        {
            return type switch
            {
                ErrorType.Failure => "https://tools.ietf.org/html/rfc7231#section-6.6.1", // 500 Internal Server Error
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1", // 400 Bad Request
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8", // 409 Conflict
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4", // 404 Not Found
                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1" // Default to 500 Internal Server Error
            };
        }
        public static T Match<T>(this Result result, Func<T> onSuccess, Func<Errors, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }

        public static T Result<T>(this Result result, Func<T> onSuccess, Func<Errors, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }
    }
}
