using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<UserListDto>> GetUsersAsync()
    {
        var users = await _repository.GetAllAsync();

        return users.Select(u => new UserListDto
        {
            Id = u.Id,
            Name = u.Name,
            Login = u.Login,
            Role = u.Role.ToString(),
            Company = u.CompanyId ?? 0
        }).ToList();
    }

    public async Task<UserDetailsDto?> GetUserAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        return new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Initials = user.Initials,
            Login = user.Login,
            Role = user.Role.ToString(),
            Company = user.CompanyId ?? 0
        };
    }

    public async Task CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Name = dto.Name,
            Initials = dto.Initials,
            Login = dto.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _repository.AddAsync(user);
    }
}
