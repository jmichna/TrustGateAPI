namespace TrustGateAPI.Services.Interfaces;

public interface ICsvEndpointImportService
{
    Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file);
}