namespace DsK.Services.Email;
public interface IMailService
{
    Task SendAsync(MailRequest request);
}
