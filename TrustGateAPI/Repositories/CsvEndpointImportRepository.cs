using Microsoft.EntityFrameworkCore;
using TrustGateCore.Models;
using TrustGateCore.ModelsDto;
using TrustGateAPI.Validation;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Repositories;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class CsvEndpointImportRepository(SqlDbContext db, ICsvReaderRepository csvReader)
    : ICsvEndpointImportRepository
{
    private readonly SqlDbContext _db;
    private readonly ICsvReaderRepository _csvReader;

    public async Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file)
    {
        CsvFileValidator.ValidateImportFile(file);

        var rows = await _csvReader.ReadAsync(file);

        var companyCache = new Dictionary<string, Company>();
        var endpointsToAdd = BuildEndpoints(rows, companyCache);

        if (endpointsToAdd.Count > 0)
            await _db.ApiEndpoints.AddRangeAsync(endpointsToAdd);

        return await _db.SaveChangesAsync();
    }

    private List<ApiEndpoint> BuildEndpoints(
        IReadOnlyList<CsvRowDto> rows,
        Dictionary<string, Company> companyCache)
    {
        var endpointsToAdd = new List<ApiEndpoint>();

        foreach (var row in rows)
        {
            var company = GetOrCreateCompany(row, companyCache);
            if (company is null)
                continue;

            var endpoint = CreateEndpointIfValid(row, company);
            if (endpoint is not null)
                endpointsToAdd.Add(endpoint);
        }

        return endpointsToAdd;
    }

    private Company? GetOrCreateCompany(
        CsvRowDto row,
        Dictionary<string, Company> companyCache)
    {
        row.Columns.TryGetValue("NazwaFirmy", out var companyName);
        row.Columns.TryGetValue("InicjalyFirmy", out var companyInitials);
        row.Columns.TryGetValue("NazwaProjektu", out var projectName);
        row.Columns.TryGetValue("IdProjektu", out var projectIdString);

        if (string.IsNullOrWhiteSpace(companyName))
            return null;

        var nameNorm = companyName.Trim();
        var initialsNorm = (companyInitials ?? string.Empty).Trim();
        var projectNameNorm = (projectName ?? string.Empty).Trim();

        var key = $"{nameNorm.ToUpperInvariant()}|" +
                  $"{initialsNorm.ToUpperInvariant()}|" +
                  $"{projectNameNorm.ToUpperInvariant()}";

        int projectId = 0;
        if (!string.IsNullOrWhiteSpace(projectIdString))
            int.TryParse(projectIdString, out projectId);

        if (companyCache.TryGetValue(key, out var cachedCompany))
            return cachedCompany;

        var company = _db.Companies.FirstOrDefault(c =>
            c.CompanyName == nameNorm &&
            c.CompanyInitials == initialsNorm &&
            c.ProjectName == projectNameNorm);

        if (company is null)
        {
            company = new Company
            {
                CompanyName = nameNorm,
                CompanyInitials = initialsNorm,
                ProjectName = projectNameNorm,
                ProjectId = projectId
            };

            _db.Companies.Add(company);
        }

        companyCache[key] = company;
        return company;
    }

    private ApiEndpoint? CreateEndpointIfValid(CsvRowDto row, Company company)
    {
        row.Columns.TryGetValue("EndpointName", out var endpointName);
        row.Columns.TryGetValue("HttpMethod", out var httpMethod);
        row.Columns.TryGetValue("Route", out var route);

        if (string.IsNullOrWhiteSpace(endpointName) ||
            string.IsNullOrWhiteSpace(route))
            return null;

        return new ApiEndpoint
        {
            Name = endpointName.Trim(),
            HttpMethod = string.IsNullOrWhiteSpace(httpMethod)
                ? "GET"
                : httpMethod.Trim().ToUpperInvariant(),
            Route = route.Trim(),
            Company = company
        };
    }
}
