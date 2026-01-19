using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.ModelsDto;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Controllers;

[Authorize(Roles = "Admin")]
public class CompaniesController : BaseController
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }


    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }


    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateCompanyDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
