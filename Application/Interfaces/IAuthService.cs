using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResponse> RegisterAsync(RegisterDTO dto);
}

public class ServiceResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}