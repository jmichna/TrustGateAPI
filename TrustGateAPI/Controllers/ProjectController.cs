using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Security;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

public class ProjectController : BaseController
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet("company/getByCompany")]
    [Authorize(Roles = "User,Company")]
    public async Task<IActionResult> GetByCompany()
    {
        var companyId = User.GetCompanyId();
        var projects = await _service.GetByCompanyIdAsync(companyId);
        return Ok(projects);
    }

    [HttpGet("admin/getAll")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects);
    }
}
