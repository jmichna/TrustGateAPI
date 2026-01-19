using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

public class UsersController : BaseController
{
    private readonly IUserForCompanyService _companyService;
    private readonly IUserService _service;

    public UsersController(IUserForCompanyService companyService, IUserService service)
    {
        _companyService = companyService;
        _service = service;
    }


    [Authorize(Roles = "Company")]
    [HttpGet("company/users")]
    public async Task<IActionResult> GetCompanyUsers()
    {
        var companyId = int.Parse(User.FindFirst("companyId")!.Value);
        return Ok(await _companyService.GetUsersForCompanyAsync(companyId));
    }

    [Authorize]
    [HttpPost("company/users")]
    public async Task<IActionResult> CreateCompanyUser(CreateUserDto dto)
    {
        var companyId = int.Parse(User.FindFirst("companyId")!.Value);
        await _companyService.CreateUserForCompanyAsync(dto, companyId);
        return Ok();
    }

    [Authorize(Roles = "Company")]
    [HttpDelete("company/users/{id}")]
    public async Task<IActionResult> DeleteCompanyUser(int id)
    {
        var companyId = int.Parse(User.FindFirst("companyId")!.Value);
        await _companyService.DeleteUserForCompanyAsync(id, companyId);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/users")]
    public async Task<IActionResult> GetAllUsersForAdmin()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/user/{id}")]
    public async Task<UserDetailsDto?> GetUserAsync(int id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null) return null;

        return new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Initials = user.Initials,
            Login = user.Login,
            Role = user.Role.ToString()
        };
    }
}
