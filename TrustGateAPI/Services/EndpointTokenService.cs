using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class EndpointTokenService(IEndpointTokenRepository repository, IConfiguration configuration) : IEndpointTokenService
{
    private readonly IEndpointTokenRepository _repository = repository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<(ApiToken token, IReadOnlyList<ApiEndpoint> endpoints)>
        GenerateTokenForEndpointsAsync(IReadOnlyList<int> endpointIds)
    {
        var endpoints = await _repository.GetEndpointsByIdsAsync(endpointIds);

        if (endpoints.Count == 0)
            throw new ArgumentException("No endpoints found for provided IDs.");

        var jwt = GenerateJwt();
        DateTime? expiresAt = DateTime.UtcNow.AddHours(
            int.TryParse(_configuration["JsonSettings:TokenExpirationHours"], out var h) ? h : 24
        );

        var token = await _repository.CreateTokenAsync(jwt, expiresAt);

        await _repository.AssignTokenToEndpointsAsync(token.Id, endpoints);

        return (token, endpoints);
    }

    private string GenerateJwt()
    {
        var section = _configuration.GetSection("JsonSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["JwtKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("type", "endpoint-token")
        };

        var token = new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}