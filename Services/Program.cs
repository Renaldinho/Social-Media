using MailKit.Net.Smtp;
using MimeKit;
using Services;

class Program
{
    static void Main(string[] args)
    {
        EmailService emailService = new EmailService();
        emailService.SendEmail("caca", "caca");
    }
}