using Khourse.Api.Common;
using Khourse.Api.Configs;
using Khourse.Api.Dtos;
using Khourse.Api.Dtos.Account;
using Khourse.Api.Services.Email;
using Khourse.Api.Services.Email.IEmail;
using Khourse.Api.Services.IServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Khourse.Api.Controllers;


[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/accounts")]
public class AccountController(IAccountService accountService, IEmailQueue emailQueue) : BaseController
{

    private readonly IAccountService _accountService = accountService;
    private readonly IEmailQueue _emailQueue = emailQueue;

    [HttpPost("register")]
    public async Task<IActionResult> Signup([FromBody] RegisterDto registerDto)
    {
        var user = await _accountService.RegisterAccount(registerDto);
        var message = new EmailDto
        {
            To = registerDto.Email!,
            Subject = "Welcome to Khourse",
            TemplateName = "Welcome.cshtml",
            Model = new
            {
                Name = registerDto.FirstName,
                ActionUrl = "#"
            }
        };
        await _emailQueue.QueueEmailAsync(message);
        return OkResponse("User created successfully", user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _accountService.LoginAccount(loginDto);
        return OkResponse("User logged in successfully", user);
    }

    [HttpPatch("update_role/{userId}")]
    public async Task<IActionResult> UpdateUserRole([FromRoute] string userId, [FromBody] UpdateRoleDto roleDto)
    {
        if (!GuidUtils.TryParse(userId, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");
        }
        await _accountService.UpdateUserRole(roleDto, userId);
        return Ok("Role updated");
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId)
    {
        if (!GuidUtils.TryParse(userId, out Guid guid))
        {
            throw new BadHttpRequestException("Invalid Id format!");
        }
        var user = await _accountService.GetUserById(userId);
        return OkResponse("User fetched successfully", user);
    }
}
