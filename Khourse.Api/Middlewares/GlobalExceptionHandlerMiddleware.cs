using System.Text.Json;
using Khourse.Api.Common;
using Khourse.Api.Exceptions;

namespace Khourse.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";

            var (statusCode, errorType) = GetStatusAndTypeFromException(ex);

            context.Response.StatusCode = statusCode;

            ApiErrorResponse errorResponse;

            if (ex is IdentityErrorException identityEx)
            {
                errorResponse = ApiErrorResponse.Fail(
                    statusCode,
                    identityEx.Message,
                    identityEx.Errors.Select(e => new ErrorDetail { Code = e.Code, Description = e.Description })
                );
            }
            else
            {
                errorResponse = ApiErrorResponse.Fail(
                    statusCode,
                    errorType,
                    [
                        new() { Code = errorType.Replace(" ", ""), Description = ex.Message }
                    ]
                );
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }

    private static (int StatusCode, string ErrorType) GetStatusAndTypeFromException(Exception ex) => ex switch
    {
        IdentityErrorException => (StatusCodes.Status400BadRequest, "Validation Error"),
        BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
        ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request"),
        FormatException => (StatusCodes.Status400BadRequest, "Bad Request"),
        InvalidCastException => (StatusCodes.Status400BadRequest, "Bad Request"),

        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
        AccessViolationException => (StatusCodes.Status403Forbidden, "Forbidden"),
        KeyNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
        FileNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
        DirectoryNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),

        NotSupportedException => (StatusCodes.Status405MethodNotAllowed, "Method Not Allowed"),

        InvalidOperationException => (StatusCodes.Status409Conflict, "Conflict"),

        _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
    };
}
