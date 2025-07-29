using Khourse.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult OkResponse<T>(string message, T data)
    {
        return Ok(ApiSuccessResponse<T>.Ok(data, message));
    }
}
