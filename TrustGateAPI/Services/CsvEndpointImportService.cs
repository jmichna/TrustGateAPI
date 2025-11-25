using Microsoft.AspNetCore.Http;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Services;

public class CsvEndpointImportService(ICsvEndpointImportRepository repository)
    : ICsvEndpointImportService
{
    public Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file)
        => repository.ImportCompaniesWithEndpointsAsync(file);
}