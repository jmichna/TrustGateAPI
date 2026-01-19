using TrustGateCore.Models;

namespace TrustGateAPI.Services.Interfaces;

public interface IApiEndpointSubscriptionService
{
    Task SubscribeAsync(int tokenId, int endpointId);
    Task<List<ApiEndpoint>> GetEndpointsForTokenAsync(int tokenId);
    Task RemoveAsync(int tokenId, int endpointId);
}
