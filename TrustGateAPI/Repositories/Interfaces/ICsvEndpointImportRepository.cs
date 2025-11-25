namespace TrustGateAPI.Repositories.Interfaces;

public interface ICsvEndpointImportRepository
{
    Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file);
}
