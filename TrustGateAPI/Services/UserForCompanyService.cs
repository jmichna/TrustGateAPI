using TrustGateAPI.Enums;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class UserForCompanyService : IUserForCompanyService
{
    private readonly IUserRepository _repository;

    public UserForCompanyService(IUserRepository repository)
    {
        _repository = repository;
    }

    // ===============================
    // LISTA USERÓW FIRMY
    // ===============================
    public async Task<List<UserListDto>> GetUsersForCompanyAsync(int companyId)
    {
        var users = await _repository.GetByCompanyIdAsync(companyId);

        return users.Select(u => new UserListDto
        {
            Id = u.Id,
            Name = u.Name,
            Login = u.Login,
            Role = u.Role.ToString()
        }).ToList();
    }

    public async Task CreateUserForCompanyAsync(CreateUserDto dto, int companyId)
    {
        if (dto.Role != UserRole.User)
            throw new UnauthorizedAccessException(
                "Company can create only users with role User"
            );

        var user = new User
        {
            Name = dto.Name,
            Initials = dto.Initials,
            Login = dto.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.User,
            CompanyId = companyId
        };

        await _repository.AddAsync(user);
    }

    public async Task DeleteUserForCompanyAsync(int userId, int companyId)
    {
        var user = await _repository.GetByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        if (user.CompanyId != companyId)
            throw new UnauthorizedAccessException("User from another company");

        if (user.Role != UserRole.User)
            throw new UnauthorizedAccessException("Cannot delete non-user role");

        await _repository.DeleteAsync(user);
    }
}