using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class ApiTokenRepository : IApiTokenRepository
{
    private readonly SqlDbContext _context;

    public ApiTokenRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<ApiToken> AddAsync(ApiToken token)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == token.ProjectId);

        if (!projectExists)
            throw new InvalidOperationException(
                $"Project with id {token.ProjectId} does not exist");

        _context.ApiTokens.Add(token);
        await _context.SaveChangesAsync();

        return token;
    }

    public async Task<ApiToken?> GetByIdAsync(int id)
    {
        return await _context.ApiTokens
            .Include(t => t.Project)
            .Include(t => t.ApiEndpointTokens)
                .ThenInclude(et => et.ApiEndpoint)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<ApiToken>> GetAllAsync()
    {
        return await _context.ApiTokens
            .Include(t => t.Project)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<ApiToken>> GetForCompanyAsync(int companyId)
    {
        return await _context.ApiTokens
            .Include(t => t.Project)
            .Where(t => t.Project.CompanyId == companyId)
            .AsNoTracking()
            .ToListAsync();
    }
}
