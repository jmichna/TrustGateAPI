using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class ApiTokenAccessRepository : IApiTokenAccessRepository
{
    private readonly SqlDbContext _context;

    public ApiTokenAccessRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<ApiToken?> GetValidApiTokenAsync(string token)
    {
        return await _context.ApiTokens
            .Include(t => t.ApiEndpointTokens)
                .ThenInclude(et => et.ApiEndpoint)
            .FirstOrDefaultAsync(t =>
                t.Token == token &&
                t.IsActive &&
                t.ExpiresAt > DateTime.UtcNow);
    }
}
