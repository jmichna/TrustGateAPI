using TrustGateAPI.ModelsDto;

namespace TrustGateAPI.Services.Interfaces;

public interface IUserForCompanyService
{
    Task<List<UserListDto>> GetUsersForCompanyAsync(int companyId);
    Task CreateUserForCompanyAsync(CreateUserDto dto, int companyId);
    Task DeleteUserForCompanyAsync(int userId, int companyId);
}
