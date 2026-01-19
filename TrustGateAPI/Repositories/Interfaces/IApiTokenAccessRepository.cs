using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IApiTokenAccessRepository
{
    Task<ApiToken?> GetValidApiTokenAsync(string token);
}
