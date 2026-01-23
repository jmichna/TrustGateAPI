using TrustGateAPI.ModelsDto;

namespace TrustGateAPI.Services.Interfaces;

public interface ICompanyService
{
    Task<List<CompanyListDto>> GetAllAsync();
    Task CreateAsync(CreateCompanyDto dto);
    Task DeleteAsync(int id);
}
