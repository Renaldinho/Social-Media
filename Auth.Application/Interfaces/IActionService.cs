namespace Application.Interfaces;

public interface IActionService
{
    void SendSuccessfulRegistrationEmail(string email);

    void SendCreateUserMessage(int id);
}