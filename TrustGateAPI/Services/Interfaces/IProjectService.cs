using TrustGateAPI.ModelsDto;

namespace TrustGateAPI.Services.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync();
    Task<List<ProjectDto>> GetByCompanyIdAsync(int companyId);
}