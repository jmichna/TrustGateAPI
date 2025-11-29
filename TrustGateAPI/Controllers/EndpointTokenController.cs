using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Management.Smo.Wmi;
using TrustGateAPI.Contracts;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

[Authorize]
public class EndpointTokenController(IEndpointTokenService service) : BaseController
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateEndpointTokenRequest request)
    {
        if (request.EndpointIds.Count == 0)
            return BadRequest("EndpointIds is required.");

        var (token, endpoints) = await service.GenerateTokenForEndpointsAsync(request.EndpointIds);

        return Ok(new
        {
            TokenId = token.Id,
            Token = token.Value,
            Endpoints = endpoints.Select(e => new { e.Id, e.Name, e.Route })
        });
    }
}

