using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrustGateAPI.Models.Settings;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly JsonSetting _settings;
    private readonly byte[] _secretKey;
        
    public AuthorizationService(IOptions<JsonSetting> jsonSettings)
    {
        _settings = jsonSettings.Value;
        _secretKey = Encoding.UTF8.GetBytes(_settings.JwtKey);
    }

    public string GenerateToken(string login, string password)
    {
        if(login != "admin" || password != "password")
        {
            throw new UnauthorizedAccessException("Invalid login or password");
        }

        var email = $"{login.ToLower()}@example.com";
        return CreateToken(login, email);
    }

    public string RefreshToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
            }, out _);

            var login = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email))
                throw new SecurityTokenException("Invalid token claims.");

            return CreateToken(login, email);
        }
        catch (SecurityTokenExpiredException)
        {
            throw new SecurityTokenException("Token has expired. Please login again.");
        }
        catch (Exception)
        {
            throw new SecurityTokenException("Invalid or malformed token.");
        }
    }

    private string CreateToken(string login, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, login),
            new Claim(ClaimTypes.Email, email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_settings.TokenExpirationHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secretKey),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }
}
