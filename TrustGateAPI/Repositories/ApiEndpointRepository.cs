using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class ApiEndpointRepository : IApiEndpointRepository
{
    private readonly SqlDbContext _context;

    public ApiEndpointRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApiEndpoint>> GetAllAsync()
    {
        return await _context.ApiEndpoints.ToListAsync();
    }

    public async Task<List<ApiEndpoint>> GetForCompanyAsync(int companyId)
    {
        return await _context.ApiEndpoints
        .Include(e => e.Project)
        .Where(e => e.Project.CompanyId == companyId)
        .ToListAsync();
    }
}
