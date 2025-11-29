using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class EndpointTokenRepository(SqlDbContext db) : IEndpointTokenRepository
{
    private readonly SqlDbContext _db = db;

    public async Task<ApiToken> CreateTokenAsync(string value, DateTime? expiresAt)
    {
        var token = new ApiToken
        {
            Value = value,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            Status = "Generated"
        };

        _db.ApiTokens.Add(token);
        await _db.SaveChangesAsync();

        return token;
    }

    public async Task<IReadOnlyList<ApiEndpoint>> GetEndpointsByIdsAsync(IReadOnlyList<int> ids)
    {
        return await _db.ApiEndpoints
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public async Task AssignTokenToEndpointsAsync(int tokenId, IReadOnlyList<ApiEndpoint> endpoints)
    {
        foreach (var ep in endpoints)
            ep.ApiTokenId = tokenId;

        await _db.SaveChangesAsync();
    }
}
