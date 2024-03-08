using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Context;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService: ITokenService
{
    private readonly JwtConfig _config;

    public TokenService(JwtConfig config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                // Add other claims as needed
            }),
            Expires = DateTime.UtcNow.AddHours(1), // Set the token to expire
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}