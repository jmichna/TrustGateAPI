using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IApiEndpointRepository
{
    Task<List<ApiEndpoint>> GetAllAsync();
    Task<List<ApiEndpoint>> GetForCompanyAsync(int companyId);
}