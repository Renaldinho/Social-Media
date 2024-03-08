using Application.DTOs;
using Application.Interfaces;
using Application.JWT;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result.Success)
            {
                return Ok(new { Message = "Registration successful. A security email has been sent out." });
            }
            return BadRequest(new { result.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var serviceResponse = await _authService.LoginAsync(loginDto);
            if (serviceResponse.Success)
            {
                return Ok(new { Token = serviceResponse.Data, Message = "Login successful" });
            }
            return Unauthorized("Authentication failed.");
        }
        catch (Exception e)
        {
            // Log the exception details here instead of console to maintain clean Auth.API responses.
            // Consider using a logging framework or service.
            // For the sake of error handling, return a more generic error message to the client.
            return StatusCode(500, "An internal error occurred.");
        }
    }
}