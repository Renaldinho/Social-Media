

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

    public EmailService()
    {
        _receiverEmail = "renarsmednieks13@gmail.com";
        _smtpServer = "smtp.gmail.com";
        _smtpPort = 587;
        _smtpUsername = "taterclientservice@gmail.com";
        _smtpPassword = "sehu yvbm vxrv ibzj";
    }

    public async Task SendEmail(string email, string message)
    {
        using (var smtp = new SmtpClient())
        {
            Console.WriteLine("Sending email");
            
            await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Twooter Inc", _smtpUsername));
            mailMessage.To.Add(MailboxAddress.Parse(email));
            mailMessage.Subject = "Account registration"; 
            mailMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            await smtp.SendAsync(mailMessage);
            
            Console.WriteLine("Email has been delivered to" + email);
            smtp.Disconnect(true);
        }
    }
}
