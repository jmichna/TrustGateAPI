namespace TrustGateAPI.Services.Interfaces;

public interface IApiTokenAccessService
{
    Task<bool> HasAccessAsync(
        string token,
        string httpMethod,
        string route,
        int companyId
    );
}
