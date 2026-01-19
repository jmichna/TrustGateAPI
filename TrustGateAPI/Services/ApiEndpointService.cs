using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;

namespace TrustGateAPI.Services;

public class ApiEndpointService : IApiEndpointService
{
    private readonly IApiEndpointRepository _repository;

    public ApiEndpointService(IApiEndpointRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ApiEndpoint>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<List<ApiEndpoint>> GetForCompanyAsync(int companyId)
    {
        return await _repository.GetForCompanyAsync(companyId);
    }
}
