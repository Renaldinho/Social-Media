using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository: IUserRepository
{

    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<bool> UserExists(string email)
    {
        _databaseContext.Database.EnsureCreated();
        return _databaseContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task AddUser(User user)
    {
        _databaseContext.Add(user);
        await _databaseContext.SaveChangesAsync();
    }

    public Task<User?> FindByEmail(string email)
    {
        return _databaseContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}