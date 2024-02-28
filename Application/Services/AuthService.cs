using Application.DTOs;
using Application.Interfaces;
using Application.JWT;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class AuthService: IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ServiceResponse> RegisterAsync(RegisterDTO dto)
    {
        if(await _userRepository.UserExists(dto.Email))
        {
            return new ServiceResponse { Success = false, Message = "Email is already in use." };
        } 
        
        CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await _userRepository.AddUser(user);

        return new ServiceResponse { Success = true, Message = "User registered successfully." };
        
    }

    public Task<BearerToken> LoginAsync(LoginDTO loginDto)
    {
        
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}