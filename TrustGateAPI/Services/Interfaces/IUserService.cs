using TrustGateAPI.ModelsDto;

namespace TrustGateAPI.Services.Interfaces;

public interface IUserService
{
    Task<List<UserListDto>> GetUsersAsync();
    Task<UserDetailsDto?> GetUserAsync(int id);
    Task CreateUserAsync(CreateUserDto dto);
}
