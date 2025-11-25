using TrustGateAPI.Repositories;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services;
using TrustGateAPI.Services.Interfaces;

namespace TrustGateAPI.Configurations;

public static class ServiceConfig
{
    public static IServiceCollection AddProjectService(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        
        // CSV parsing
        services.AddScoped<ICsvReaderRepository, CsvReaderRepository>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();

        // CSV import -> DB
        services.AddScoped<ICsvEndpointImportRepository, CsvEndpointImportRepository>();
        services.AddScoped<ICsvEndpointImportService, CsvEndpointImportService>();

        return services;
    }
}
