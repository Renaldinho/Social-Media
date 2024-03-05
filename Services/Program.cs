using MailKit.Net.Smtp;
using MimeKit;

class Program
{
    static void Main(string[] args)
    {
        string receiverEmail = "renarsmednieks13@gmail.com"; // Set the receiver's email address here
        string SmtpUsername = "taterclientservice@gmail.com"; // Your Gmail address
        string SmtpPassword = "sehu yvbm vxrv ibzj"; // Your app-specific password or password

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("WhereHouse Inc", SmtpUsername));
        email.To.Add(MailboxAddress.Parse(receiverEmail));
        email.Subject = "Hello!";
        email.Body = new TextPart("plain")
        {
            Text = "Hi"
        };

        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(SmtpUsername, SmtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        
        Console.WriteLine($"Email sent to {receiverEmail}");
    }
}