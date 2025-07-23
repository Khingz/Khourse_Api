using Khourse.Api.Dtos.Account;
using Khourse.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/accounts")]
public class AccountController(IAccountService accountService) : BaseController
{

    private readonly IAccountService _accountService = accountService;
    [HttpPost("register")]
    public async Task<IActionResult> Signup([FromBody] RegisterDto registerDto)
    {
        var (success, user, errors) = await _accountService.RegisterAccount(registerDto);

        if (!success)
        {
            // return InternalServerErrorResponse("Something went wrong");
            return BadRequest(new
            {
                success = false,
                message = "Validation failed",
                errors = errors.Select(e => new { e.Code, e.Description })
            });
        }
        return OkResponse("User created successfully", user);

    }
}
