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
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IEndpointTokenService, EndpointTokenService>();
        services.AddScoped<ICsvEndpointImportService, CsvEndpointImportService>();

        // CSV import -> DB
        services.AddScoped<ICsvReaderRepository, CsvReaderRepository>();
        services.AddScoped<ICsvEndpointImportRepository, CsvEndpointImportRepository>();
        services.AddScoped<IEndpointTokenRepository, EndpointTokenRepository>();

        // Generate Token for Endpoints

        return services;
    }
}
