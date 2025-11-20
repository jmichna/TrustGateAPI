namespace TrustGateAPI.Repositories.Interfaces;

public interface ICsvEndpointRepository
{
    Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file);
}
