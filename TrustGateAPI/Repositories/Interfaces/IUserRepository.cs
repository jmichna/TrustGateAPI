using TrustGateCore.Models;

namespace TrustGateAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<List<User>> GetByCompanyIdAsync(int companyId);
    Task AddAsync(User user);
    Task DeleteAsync(User user);
}