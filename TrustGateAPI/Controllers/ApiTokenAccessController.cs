using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

public class ApiTokenAccessController : BaseController
{
    private readonly IApiTokenAccessService _service;

    public ApiTokenAccessController(IApiTokenAccessService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/check")]
    public async Task<IActionResult> CheckAccess(
        [FromBody] CheckAccessRequest request)
    {
        try
        {
            var hasAccess = await _service.HasAccessAsync(
                request.Token,
                request.HttpMethod,
                request.Route,
                request.CompanyId
            );
            return Ok(new { hasAccess });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}