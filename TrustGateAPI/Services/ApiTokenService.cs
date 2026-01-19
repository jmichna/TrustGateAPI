using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrustGateAPI.Models.Settings;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class ApiTokenService : IApiTokenService
{
    private readonly IApiTokenRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly JsonSetting _settings;

    public ApiTokenService(
        IApiTokenRepository repository,
        IProjectRepository projectRepository,
        IOptions<JsonSetting> settings)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _settings = settings.Value;
    }

    public async Task<ApiToken> GenerateAsync(
    int projectId,
    int companyId,
    int validDays)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);

        if (project == null)
            throw new KeyNotFoundException("Project not found");

        if (project.CompanyId != companyId)
            throw new UnauthorizedAccessException(
                "Cannot generate token for another company project"
            );

        var expiresAt = DateTime.UtcNow.AddDays(validDays);
        var jwt = GenerateJwt(expiresAt);

        var apiToken = new ApiToken
        {
            Token = jwt,
            ExpiresAt = expiresAt,
            IsActive = true,
            ProjectId = projectId
        };

        return await _repository.AddAsync(apiToken);
    }

    private string GenerateJwt(DateTime expiresAt)
    {
        var claims = new[]
        {
            new Claim("type", "api"),
            new Claim("iss", "TrustGate")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.JwtKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<List<ApiTokenListDto>> GetAllAsync()
    {
        var tokens = await _repository.GetAllAsync();

        return tokens.Select(t => new ApiTokenListDto
        {
            Id = t.Id,
            Token = t.Token,
            ExpiresAt = t.ExpiresAt,
            IsActive = t.IsActive
        }).ToList();
    }

    public async Task<List<ApiTokenListDto>> GetForCompanyAsync(int companyId)
    {
        var tokens = await _repository.GetForCompanyAsync(companyId);
        return tokens.Select(MapToDto).ToList();
    }

    private static ApiTokenListDto MapToDto(ApiToken t) =>
        new()
        {
            Id = t.Id,
            Token = t.Token,
            ExpiresAt = t.ExpiresAt,
            IsActive = t.IsActive,
            ProjectId = t.ProjectId
        };
}
