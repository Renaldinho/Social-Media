using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExists(string email);

    Task AddUser(User user);

    Task<User?> FindByEmail(string email);
}