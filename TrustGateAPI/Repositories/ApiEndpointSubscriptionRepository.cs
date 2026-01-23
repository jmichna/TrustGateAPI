using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories
{
    public class ApiEndpointSubscriptionRepository : IApiEndpointSubscriptionRepository
    {
        private readonly SqlDbContext _context;

        public ApiEndpointSubscriptionRepository(SqlDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TokenExistsAsync(int tokenId)
            => await _context.ApiTokens.AnyAsync(t => t.Id == tokenId);

        public async Task<bool> EndpointExistsAsync(int endpointId)
            => await _context.ApiEndpoints.AnyAsync(e => e.Id == endpointId);

        public async Task<bool> SubscriptionExistsAsync(int tokenId, int endpointId)
            => await _context.ApiEndpointTokens
                .AnyAsync(x => x.ApiTokenId == tokenId && x.ApiEndpointId == endpointId);

        public async Task AddSubscriptionAsync(ApiEndpointToken entity)
        {
            _context.ApiEndpointTokens.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApiEndpoint>> GetEndpointsForTokenAsync(int tokenId)
        {
            return await _context.ApiEndpointTokens
                .Where(x => x.ApiTokenId == tokenId)
                .Select(x => x.ApiEndpoint)
                .ToListAsync();
        }

        public async Task RemoveAsync(int tokenId, int endpointId)
        {
            var entity = await _context.ApiEndpointTokens
                .FirstOrDefaultAsync(x =>
                    x.ApiTokenId == tokenId &&
                    x.ApiEndpointId == endpointId);

            if (entity != null)
            {
                _context.ApiEndpointTokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
