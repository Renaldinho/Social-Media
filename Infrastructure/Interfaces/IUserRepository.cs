namespace Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string email);
    
}