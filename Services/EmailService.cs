using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Services;

public class EmailService
{
    private readonly string _receiverEmail;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;

    public EmailService() // Constructor with hardcoded values
    {
        _receiverEmail = "renarsmednieks13@gmail.com"; // Replace with your actual recipient email
        _smtpServer = "smtp.gmail.com"; // Replace with your actual SMTP server
        _smtpPort = 587;
        _smtpUsername = "taterclientservice@gmail.com"; // Replace with your email address
        _smtpPassword = "sehu yvbm vxrv ibzj"; // Replace with your app-specific password or password
    }

    public void SendEmail(string subject, string body)
    {
        using (var smtp = new SmtpClient())
        {
            smtp.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpUsername, _smtpPassword);
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("WhereHouse Inc", _smtpUsername));
            email.To.Add(MailboxAddress.Parse(_receiverEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = body
            };
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        Console.WriteLine($"Email sent to {_receiverEmail}");
    }
}
