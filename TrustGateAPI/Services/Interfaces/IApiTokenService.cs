using TrustGateAPI.ModelsDto;
using TrustGateCore.Models;

namespace TrustGateAPI.Services.Interfaces;

public interface IApiTokenService
{
    Task<ApiToken> GenerateAsync(int projectId, int companyId, int validDays);
    Task<List<ApiTokenListDto>> GetAllAsync();
    Task<List<ApiTokenListDto>> GetForCompanyAsync(int companyId);
}
