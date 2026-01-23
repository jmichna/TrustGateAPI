using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class ApiEndpointTokenAssignmentRepository : IApiEndpointTokenAssignmentRepository
{
    private readonly SqlDbContext _context;

    public ApiEndpointTokenAssignmentRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<List<int>> GetAssignedEndpointIdsAsync(int tokenId)
    {
        return await _context.ApiEndpointTokens
            .Where(x => x.ApiTokenId == tokenId)
            .Select(x => x.ApiEndpointId)
            .ToListAsync();
    }

}
