using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Security;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

public class ApiEndpointTokenAssignmentController : BaseController
{
    private readonly IApiEndpointTokenAssignmentService _service;

    public ApiEndpointTokenAssignmentController(
        IApiEndpointTokenAssignmentService service)
    {
        _service = service;
    }

    [HttpGet("company/token/{tokenId}")]
    public async Task<IActionResult> GetEndpointsForCompanyToken(int tokenId)
    {
        if (User.IsAdmin())
            return Forbid();

        var companyId = User.GetCompanyId();

        var result = await _service.GetEndpointsForTokenAsync(
            companyId,
            tokenId
        );

        return Ok(result);
    }

    [HttpGet("admin/token/{tokenId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEndpointsForAdmin(int tokenId)
    {
        var result = await _service.GetEndpointsForTokenAsync(
            companyId: null,
            tokenId: tokenId
        );

        return Ok(result);
    }
}