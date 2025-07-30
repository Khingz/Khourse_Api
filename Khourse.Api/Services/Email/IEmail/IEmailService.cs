using System;
using Khourse.Api.Dtos;

namespace Khourse.Api.Services.Email.IEmail;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
}
