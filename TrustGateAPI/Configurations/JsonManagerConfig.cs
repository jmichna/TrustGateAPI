using TrustGateAPI.Models.Settings;

namespace TrustGateAPI.Configurations;

public static class JsonManagerConfig
{
    public static void ConfigureJsonSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonSetting>(configuration.GetSection("JsonSettings"));
    }
}