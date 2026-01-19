using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;

    public CompanyService(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CompanyListDto>> GetAllAsync()
    {
        var companies = await _repository.GetAllAsync();

        return companies.Select(c => new CompanyListDto
        {
            Id = c.Id,
            Name = c.Name,
            Initials = c.Initials
        }).ToList();
    }

    public async Task CreateAsync(CreateCompanyDto dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Initials = dto.Initials
        };

        await _repository.AddAsync(company);
    }

    public async Task DeleteAsync(int id)
    {
        var company = await _repository.GetByIdAsync(id);
        if (company == null)
            throw new KeyNotFoundException("Company not found");

        await _repository.DeleteAsync(company);
    }
}
