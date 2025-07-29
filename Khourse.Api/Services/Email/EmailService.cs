using Khourse.Api.Configs;
using Khourse.Api.Dtos;
using Khourse.Api.Services.Email.IEmail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Khourse.Api.Services.Email;

public class EmailService(IOptions<SmtpSettings> option) : IEmailService
{
    private readonly SmtpSettings _smtp = option.Value;

    public void SendEmail(EmailDto emailDto)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_smtp.From));
        message.To.Add(MailboxAddress.Parse(emailDto.To));
        message.Subject = emailDto.Subject;
        message.Body = new TextPart(TextFormat.Html) { Text = emailDto.Body };
        var smtp = new SmtpClient();

        try
        {
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtp.Username, _smtp.Password);
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            smtp.Disconnect(true);
            smtp.Dispose();
        }
    }
}
