using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IApiTokenRepository
{
    Task<ApiToken> AddAsync(ApiToken token);
    Task<ApiToken?> GetByIdAsync(int id);
    Task<List<ApiToken>> GetAllAsync();
    Task<List<ApiToken>> GetForCompanyAsync(int companyId);
}