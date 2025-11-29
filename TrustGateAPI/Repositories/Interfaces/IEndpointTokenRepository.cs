using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IEndpointTokenRepository
{
    Task<ApiToken> CreateTokenAsync(string value, DateTime? expiresAt);
    Task<IReadOnlyList<ApiEndpoint>> GetEndpointsByIdsAsync(IReadOnlyList<int> ids);
    Task AssignTokenToEndpointsAsync(int tokenId, IReadOnlyList<ApiEndpoint> endpoints);
}
