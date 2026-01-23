using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IApiEndpointSubscriptionRepository
{
    Task<bool> TokenExistsAsync(int tokenId);
    Task<bool> EndpointExistsAsync(int endpointId);
    Task<bool> SubscriptionExistsAsync(int tokenId, int endpointId);

    Task AddSubscriptionAsync(ApiEndpointToken entity);
    Task<List<ApiEndpoint>> GetEndpointsForTokenAsync(int tokenId);
    Task RemoveAsync(int tokenId, int endpointId);
}
