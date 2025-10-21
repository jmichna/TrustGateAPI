namespace TrustGateAPI.Models.Settings;

public class JsonSetting
{
    public string JwtKey { get; set; } = string.Empty;
    public int TokenExpirationHours { get; set; } = 24;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}