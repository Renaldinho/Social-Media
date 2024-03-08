namespace Application.Interfaces;

public interface IEmailActionService
{
    void SendSuccessfulRegistrationEmail(string email);
}