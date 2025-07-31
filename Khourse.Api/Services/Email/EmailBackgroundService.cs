using Khourse.Api.Services.Email.IEmail;

namespace Khourse.Api.Services.Email;

public class EmailBackgroundService(IEmailQueue emailQueue, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IEmailQueue _emailQueue = emailQueue;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var email in _emailQueue.ReadAllAsync(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            try
            {
                await emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
