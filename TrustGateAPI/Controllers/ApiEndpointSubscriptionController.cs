using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

[Authorize]
public class ApiEndpointSubscriptionController : BaseController
{
    private readonly IApiEndpointSubscriptionService _service;

    public ApiEndpointSubscriptionController(
        IApiEndpointSubscriptionService service)
    {
        _service = service;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe(
        [FromBody] SubscribeEndpointRequest request)
    {
        await _service.SubscribeAsync(
            request.ApiTokenId,
            request.ApiEndpointId);

        return Ok(new { message = "Endpoint subscribed to token" });
    }

    [HttpGet("{tokenId}/endpoints")]
    public async Task<IActionResult> GetEndpoints(int tokenId)
    {
        var endpoints = await _service.GetEndpointsForTokenAsync(tokenId);

        return Ok(endpoints.Select(e => new
        {
            e.Id,
            e.Name,
            e.HttpMethod,
            e.Route
        }));
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromBody] RemoveApiEndpointTokenRequest request)
    {
        await _service.RemoveAsync(
            request.ApiTokenId,
            request.ApiEndpointId);

        return NoContent();
    }
}
