namespace TrustGateAPI.Repositories.Interfaces;

public interface IApiEndpointTokenAssignmentRepository
{
    Task<List<int>> GetAssignedEndpointIdsAsync(int tokenId);
}