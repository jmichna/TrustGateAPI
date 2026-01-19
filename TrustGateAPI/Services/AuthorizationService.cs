using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrustGateAPI.Models;
using TrustGateAPI.Models.Settings;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly JsonSetting _settings;
    private readonly byte[] _secretKey;
    private readonly SqlDbContext _context;

    public AuthorizationService(
        IOptions<JsonSetting> jsonSettings,
        SqlDbContext context)
    {
        _settings = jsonSettings.Value;
        _secretKey = Encoding.UTF8.GetBytes(_settings.JwtKey);
        _context = context;
    }

    // 🔐 LOGIN
    public async Task<string> GenerateTokenAsync(string login, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Login == login);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid login or password");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid login or password");

        return CreateToken(user);
    }

    public string RefreshToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

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

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new SecurityTokenException("Invalid token claims.");

        return CreateTokenFromClaims(principal);
    }

    private string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("companyId", user.CompanyId.ToString())
        };

        return BuildToken(claims);
    }

    private string CreateTokenFromClaims(ClaimsPrincipal principal)
    {
        var claims = principal.Claims.ToArray();
        return BuildToken(claims);
    }

    private string BuildToken(IEnumerable<Claim> claims)
    {
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

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }
}
