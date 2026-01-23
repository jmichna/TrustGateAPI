using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Services;

public class ApiEndpointSubscriptionService : IApiEndpointSubscriptionService
{
    private readonly IApiEndpointSubscriptionRepository _repository;

    public ApiEndpointSubscriptionService(
        IApiEndpointSubscriptionRepository repository)
    {
        _repository = repository;
    }

    public async Task SubscribeAsync(int tokenId, int endpointId)
    {
        if (!await _repository.TokenExistsAsync(tokenId))
            throw new ArgumentException("API token does not exist");

        if (!await _repository.EndpointExistsAsync(endpointId))
            throw new ArgumentException("API endpoint does not exist");

        if (await _repository.SubscriptionExistsAsync(tokenId, endpointId))
            return;

        await _repository.AddSubscriptionAsync(new ApiEndpointToken
        {
            ApiTokenId = tokenId,
            ApiEndpointId = endpointId
        });
    }

    public async Task<List<ApiEndpoint>> GetEndpointsForTokenAsync(int tokenId)
        => await _repository.GetEndpointsForTokenAsync(tokenId);

    public async Task RemoveAsync(int tokenId, int endpointId)
    {
        await _repository.RemoveAsync(tokenId, endpointId);
    }
}
