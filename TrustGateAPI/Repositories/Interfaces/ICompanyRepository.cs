using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync();
    Task AddAsync(Company company);
    Task<Company?> GetByIdAsync(int id);
    Task DeleteAsync(Company company);
}