﻿using Application.DTOs;
using Application.Interfaces;
using Application.JWT;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services;

public class AuthService: IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly ITokenService _tokenService;
    private readonly IActionService _actionService;

    public AuthService(IUserRepository userRepository, IEncryptionService encryptionService, ITokenService tokenService, IActionService actionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
        _tokenService = tokenService;
        _actionService = actionService;
    }

    public async Task<ServiceResponse> RegisterAsync(RegisterDTO dto)
    {
        if(await _userRepository.UserExists(dto.Email))
        {
            return new ServiceResponse { Success = false, Message = "Email is already in use." };
        } 
        
        _encryptionService.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        await _userRepository.AddUser(user);
        
        _actionService.SendSuccessfulRegistrationEmail(user.Email);
        _actionService.SendCreateUserMessage(user.Id);

        return new ServiceResponse { Success = true, Message = "User registered successfully." };
        
    }

    public async Task<ServiceResponse> LoginAsync(LoginDTO loginDto)
    {
        var user = await _userRepository.FindByEmail(loginDto.Email);
        if (user == null)
        {
            return new ServiceResponse { Success = false, Message = "User not found." };
        }

        var passwordValid = _encryptionService.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt);
        if (!passwordValid)
        {
            return new ServiceResponse { Success = false, Message = "Credentials are incorrect." };
        }

        var token = _tokenService.GenerateToken(user);
        return new ServiceResponse { Success = true, Data = token, Message = "Login successful." };
    }

}