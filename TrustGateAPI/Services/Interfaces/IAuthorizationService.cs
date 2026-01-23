namespace TrustGateAPI.Services.Interfaces;

public interface IAuthorizationService
{
    Task<string> GenerateTokenAsync(string login, string password);
    string RefreshToken(string token);
}
