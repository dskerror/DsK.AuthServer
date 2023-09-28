using System.Net;
using System.Net.Mail;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Sending email...");
        SendMail("Test Email", "Test EmailBody");
    }

    public static void SendMail(string body, string subject)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp-relay.sendinblue.com");
        mail.From = new MailAddress("noreply@dsk.com");
        mail.To.Add("dskerror@gmail.com");
        //foreach (string item in Properties.Settings.Default.EmailTo.ToString().Split(';'))
        //{
        //    mail.To.Add(item);
        //};

        //foreach (string item in Properties.Settings.Default.EmailCc.ToString().Split(';'))
        //{
        //    mail.CC.Add(item);
        //};

        //if (EmailAttachment != "") mail.Attachments.Add(new System.Net.Mail.Attachment(EmailAttachment));

        mail.Subject = subject;
        mail.Body = body;

        mail.IsBodyHtml = true;

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new NetworkCredential("dskerror@gmail.com", "SEB5KP6qV4QGmfJz");
        SmtpServer.EnableSsl = true;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Send(mail);
        mail.Dispose();
    }

}