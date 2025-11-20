using Microsoft.AspNetCore.Http;
using TrustGateAPI.Repositories;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Services;

public class CsvEndpointImportService : ICsvEndpointImportService
{
    private readonly ICsvEndpointRepository _repository;

    public CsvEndpointImportService(ICsvEndpointRepository repository)
    {
        _repository = repository;
    }

    public Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file)
        => _repository.ImportCompaniesWithEndpointsAsync(file);
}