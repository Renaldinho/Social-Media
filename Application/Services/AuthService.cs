using Application.DTOs;
using Application.Interfaces;
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
    }
}