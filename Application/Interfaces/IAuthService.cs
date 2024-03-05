using Application.DTOs;
using Application.JWT;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse> RegisterAsync(RegisterDTO dto);
    Task<ServiceResponse> LoginAsync(LoginDTO loginDto);
}

public class ServiceResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public Object Data { get; set; }
}