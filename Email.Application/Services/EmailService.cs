

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Services.Interfaces;

namespace Services;

public class EmailService: IEmailService
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

    public async Task SendEmail(string email, string message)
    {
        using (var smtp = new SmtpClient())
        {
            Console.WriteLine("Sending email");
            
            await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("WhereHouse Inc", _smtpUsername));
            mailMessage.To.Add(MailboxAddress.Parse(email));
            mailMessage.Subject = "Your Subject Here"; // Replace with dynamic subject
            mailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            await smtp.SendAsync(mailMessage);
            
            Console.WriteLine("Sent email");
            smtp.Disconnect(true);
        }
    }
}
