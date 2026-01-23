using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;
using TrustGateAPI.Security;
using Microsoft.AspNetCore.Authorization;

namespace TrustGateAPI.Controllers;

public class ApiTokenController : BaseController
{
    private readonly IApiTokenService _service;

    public ApiTokenController(IApiTokenService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("{projectId}/generateToken")]
    public async Task<IActionResult> Generate(
        int projectId,
        [FromBody] CreateApiTokenRequest request)
    {
        var companyId = User.GetCompanyId(); // z JWT

        var token = await _service.GenerateAsync(
            projectId,
            companyId,
            request.ValidDays
        );

        return Ok(new
        {
            token.Id,
            token.Token,
            token.ExpiresAt
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/getAllInList")]
    public async Task<IActionResult> GetAll()
    {
        var tokens = await _service.GetAllAsync();
        return Ok(tokens);
    }

    [Authorize(Roles = "Company,User")]
    [HttpGet("company/getByCompanyId")]
    public async Task<IActionResult> GetByCompanyIdTokens()
    {
        var companyId = User.GetCompanyId();
        return Ok(await _service.GetForCompanyAsync(companyId));
    }
}
