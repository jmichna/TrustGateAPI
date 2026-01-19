using TrustGateAPI.Factories;
using TrustGateAPI.Factories.Interfaces;
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
        services.AddScoped<ICompanyFactory, CompanyFactory>();

        // CSV parsing
        services.AddScoped<ICsvReaderRepository, CsvReaderRepository>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();

        // CSV import -> DB
        services.AddScoped<ICsvEndpointImportRepository, CsvEndpointImportRepository>();
        services.AddScoped<ICsvEndpointImportService, CsvEndpointImportService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IApiEndpointSubscriptionRepository, ApiEndpointSubscriptionRepository>();
        services.AddScoped<IApiEndpointSubscriptionService, ApiEndpointSubscriptionService>();

        services.AddScoped<IApiTokenAccessRepository, ApiTokenAccessRepository>();
        services.AddScoped<IApiTokenAccessService, ApiTokenAccessService>();

        services.AddScoped<IApiTokenRepository, ApiTokenRepository>();
        services.AddScoped<IApiTokenService, ApiTokenService>();

        services.AddScoped<IApiEndpointRepository, ApiEndpointRepository>();
        services.AddScoped<IApiEndpointService, ApiEndpointService>();

        services.AddScoped<IApiEndpointTokenAssignmentRepository, ApiEndpointTokenAssignmentRepository>();
        services.AddScoped<IApiEndpointTokenAssignmentService, ApiEndpointTokenAssignmentService>();

        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectService, ProjectService>();

        services.AddScoped<IUserForCompanyService, UserForCompanyService>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        return services;
    }
}
