using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Controllers;

public class ApiEndpointsController : BaseController
{
    private readonly IApiEndpointService _service;

    public ApiEndpointsController(IApiEndpointService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/getAll")]
    public async Task<ActionResult<List<ApiEndpoint>>> GetAll()
    {
        var endpoints = await _service.GetAllAsync();
        return Ok(endpoints);
    }

    [Authorize(Roles = "Company,User")]
    [HttpGet("company/getByCompany")]
    public async Task<ActionResult<List<ApiEndpoint>>> GetForCompany()
    {
        var companyId = int.Parse(User.FindFirst("companyId")!.Value);

        var endpoints = await _service.GetForCompanyAsync(companyId);
        var dto = endpoints.Select(e => new ApiEndpointDto
        {
            Id = e.Id,
            Name = e.Name,
            HttpMethod = e.HttpMethod,
            Route = e.Route,
            ProjectId = e.ProjectId,
            ProjectName = e.Project.Name
        });
        return Ok(dto);
    }
}
