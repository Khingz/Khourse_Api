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
}
