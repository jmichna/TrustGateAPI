namespace TrustGateAPI.Services.Interfaces;

public interface IAuthorizationService
{
    string GenerateToken(string login, string password);
    string RefreshToken(string token);
}
