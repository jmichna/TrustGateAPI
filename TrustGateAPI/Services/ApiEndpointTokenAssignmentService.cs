using TrustGateAPI.ModelsDto;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Services;


public class ApiEndpointTokenAssignmentService : IApiEndpointTokenAssignmentService
{
    private readonly IApiEndpointRepository _endpointRepo;
    private readonly IApiEndpointTokenAssignmentRepository _assignmentRepo;

    public ApiEndpointTokenAssignmentService(
        IApiEndpointRepository endpointRepo,
        IApiEndpointTokenAssignmentRepository assignmentRepo)
    {
        _endpointRepo = endpointRepo;
        _assignmentRepo = assignmentRepo;
    }

    public async Task<List<ApiEndpointTokenAssignmentDto>> GetEndpointsForTokenAsync(
        int? companyId,
        int tokenId)
    {
        var endpoints = companyId.HasValue ? await _endpointRepo.GetForCompanyAsync(companyId.Value) : await _endpointRepo.GetAllAsync();
        var assignedIds = await _assignmentRepo.GetAssignedEndpointIdsAsync(tokenId);

        return endpoints.Select(e => new ApiEndpointTokenAssignmentDto
        {
            Id = e.Id,
            Name = e.Name,
            HttpMethod = e.HttpMethod,
            Route = e.Route,
            IsAssigned = assignedIds.Contains(e.Id)
        }).ToList();
    }
}