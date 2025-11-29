using TrustGateCore.Models;

namespace TrustGateAPI.Services.Interfaces;

public interface IEndpointTokenService
{
    Task<(ApiToken token, IReadOnlyList<ApiEndpoint> endpoints)>
        GenerateTokenForEndpointsAsync(IReadOnlyList<int> endpointIds);
}