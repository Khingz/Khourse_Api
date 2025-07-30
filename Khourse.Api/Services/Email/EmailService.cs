using Khourse.Api.Configs;
using Khourse.Api.Dtos;
using Khourse.Api.Services.Email.IEmail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RazorLight;

namespace Khourse.Api.Services.Email;

public class EmailService(IOptions<SmtpSettings> option) : IEmailService
{
    private readonly SmtpSettings _smtp = option.Value;

    public async Task SendEmailAsync(EmailDto emailDto)
    {
        var templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "Email", "Templates");
        var engine = new RazorLightEngineBuilder().UseFileSystemProject(templatesPath).UseMemoryCachingProvider().Build();

        var body = await engine.CompileRenderAsync(emailDto.TemplateName, emailDto.Model);

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_smtp.From));
        message.To.Add(MailboxAddress.Parse(emailDto.To));
        message.Subject = emailDto.Subject;
        message.Body = new TextPart(TextFormat.Html) { Text = body};
        var smtp = new SmtpClient();

        try
        {
            await smtp.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtp.Username, _smtp.Password);
            await smtp.SendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
            smtp.Dispose();
        }
    }
}
