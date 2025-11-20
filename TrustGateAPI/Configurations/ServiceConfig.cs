using TrustGateAPI.Repositories;
using TrustGateAPI.Services;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Configurations;

public static class ServiceConfig
{
    public static IServiceCollection AddProjectService(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<ICsvReaderService, CsvReaderRepository>();
        services.AddScoped<ICsvEndpointImportService, CsvEndpointRepository>();

        return services;
    }
}
