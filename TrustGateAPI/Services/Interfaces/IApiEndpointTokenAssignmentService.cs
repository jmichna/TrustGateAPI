using TrustGateAPI.ModelsDto;

namespace TrustGateAPI.Services.Interfaces;

public interface IApiEndpointTokenAssignmentService
{
    Task<List<ApiEndpointTokenAssignmentDto>> GetEndpointsForTokenAsync(
        int? companyId,
        int tokenId);
}
