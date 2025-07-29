using System;
using Khourse.Api.Dtos;

namespace Khourse.Api.Services.Email.IEmail;

public interface IEmailQueue
{
    ValueTask QueueEmailAsync(EmailDto email);
    IAsyncEnumerable<EmailDto> ReadAllAsync(CancellationToken cancellationToken);
}
