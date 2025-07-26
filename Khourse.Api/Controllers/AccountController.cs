using Khourse.Api.Dtos.Account;
using Khourse.Api.Exceptions;
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
        var user = await _accountService.RegisterAccount(registerDto);
        return OkResponse("User created successfully", user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _accountService.LoginAccount(loginDto);
        return OkResponse("User logged in successfully", user);
    }
}
