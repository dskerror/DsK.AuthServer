using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace DsK.Services.Email;
public class SMTPMailService : IMailService
{
    private readonly MailConfiguration _config;
    public SMTPMailService(IOptions<MailConfiguration> config)
    {
        _config = config.Value;
    }
    public async Task SendAsync(MailRequest request)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient(_config.Host);
            mail.From = new MailAddress(request.From ?? _config.From, _config.DisplayName);
            mail.To.Add(request.To);
            mail.Subject = request.Subject;
            mail.Body = request.Body;
            mail.IsBodyHtml = true;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(_config.UserName, _config.Password);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;            
            smtp.Send(mail);
            mail.Dispose();            
        }
        catch (Exception ex)
        {

        }
    }
}
