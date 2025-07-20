using Khourse.Api.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult OkResponse<T>(string message, T data)
    {
        return Ok(ApiResponse<T>.Ok(message, data));
    }

    protected IActionResult ErrorResponse(int code, string type, string message)
    {
        return StatusCode(code, ApiResponse<string>.Fail(code, type, message));
    }

    protected IActionResult BadRequestResponse(string message)
        => ErrorResponse(400, "Bad Request", message);

    protected IActionResult UnauthorizedResponse(string message)
        => ErrorResponse(401, "Unauthorized", message);

    protected IActionResult ForbiddenResponse(string message)
        => ErrorResponse(403, "Forbidden", message);

    protected IActionResult NotFoundResponse(string message)
        => ErrorResponse(404, "Not Found", message);

    protected IActionResult ConflictResponse(string message)
        => ErrorResponse(409, "Conflict", message);

    protected IActionResult UnprocessableEntityResponse(string message)
        => ErrorResponse(422, "Unprocessable Entity", message);

    protected IActionResult TooManyRequestsResponse(string message)
        => ErrorResponse(429, "Too Many Requests", message);

    protected IActionResult InternalServerErrorResponse(string message)
        => ErrorResponse(500, "Internal Server Error", message);
}
