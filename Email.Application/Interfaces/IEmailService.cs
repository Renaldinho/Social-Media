namespace Services.Interfaces;

public interface IEmailService
{
    public Task SendEmail(string email, string message);
}