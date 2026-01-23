using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<List<Project>> GetByCompanyIdAsync(int companyId);
    Task<Project?> GetByIdAsync(int id);
}
