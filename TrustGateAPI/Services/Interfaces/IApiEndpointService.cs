using TrustGateCore.Models;

namespace TrustGateAPI.Services.Interfaces;

public interface IApiEndpointService
{
    Task<List<ApiEndpoint>> GetAllAsync();
    Task<List<ApiEndpoint>> GetForCompanyAsync(int companyId);
}
