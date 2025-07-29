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
public class AccountController(IAccountService accountService, IOptions<SmtpSettings> options, IEmailQueue emailQueue) : BaseController
{

    private readonly IAccountService _accountService = accountService;
    private readonly SmtpSettings _smtp = options.Value;
    private readonly IEmailQueue _emailQueue = emailQueue;

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

    [HttpGet("debug/smtp")]
    public async Task<IActionResult> GetSmtpSettings([FromServices] IOptions<SmtpSettings> smtpOptions)
    {
        var message = new EmailDto
        {
            To = "soniacriag231@gmail.com",
            Subject = "Hello World",
            Body = "<p>This is a god way to start life!!</p>"
        };
        await _emailQueue.QueueEmailAsync(message);
        return Ok(new
        {
            _smtp.Host,
            _smtp.Port,
            _smtp.Username,
            _smtp.From,
            _smtp.Password
        });
    }
}
