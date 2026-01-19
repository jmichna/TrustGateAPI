using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Services;

public class ApiTokenAccessService : IApiTokenAccessService
{
    private readonly IApiTokenAccessRepository _repository;

    public ApiTokenAccessService(IApiTokenAccessRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> HasAccessAsync(
        string token,
        string httpMethod,
        string route,
        int companyId)
    {
        var apiToken = await _repository.GetValidApiTokenAsync(token);

        if (apiToken == null)
            return false;

        return apiToken.ApiEndpointTokens.Any(et =>
            et.ApiEndpoint.HttpMethod == httpMethod &&
            route.StartsWith(et.ApiEndpoint.Route) &&
            et.ApiEndpoint.Project.CompanyId == companyId
        );

    }
}
