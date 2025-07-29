using System.Threading.Channels;
using Khourse.Api.Dtos;
using Khourse.Api.Services.Email.IEmail;

namespace Khourse.Api.Services.Email;

public class EmailQueue : IEmailQueue
{
    private readonly Channel<EmailDto> _queue = Channel.CreateUnbounded<EmailDto>();

    public async ValueTask QueueEmailAsync(EmailDto email)
    {
        await _queue.Writer.WriteAsync(email);
    }

    public IAsyncEnumerable<EmailDto> ReadAllAsync(CancellationToken cancellationToken)
        => _queue.Reader.ReadAllAsync(cancellationToken);
}
