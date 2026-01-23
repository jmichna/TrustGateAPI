using Microsoft.EntityFrameworkCore;
using TrustGateCore.Models;
using TrustGateCore.ModelsDto;
using TrustGateAPI.Validation;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Repositories;
using TrustGateSqlLiteService.Db;
using TrustGateAPI.Factories.Interfaces;

namespace TrustGateAPI.Repositories;

public class CsvEndpointImportRepository(SqlDbContext db, ICsvReaderRepository csvReader, ICompanyFactory companyFactory) : ICsvEndpointImportRepository
{
    private readonly SqlDbContext _db = db;
    private readonly ICsvReaderRepository _csvReader = csvReader;
    private readonly ICompanyFactory _companyFactory = companyFactory;

    public async Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file)
    {
        CsvFileValidator.ValidateImportFile(file);

        var rows = await _csvReader.ReadAsync(file);

        var companyCache = new Dictionary<string, (Company company, Project project)>();
        var endpointsToAdd = BuildEndpoints(rows, companyCache);

        if (endpointsToAdd.Count > 0)
            await _db.ApiEndpoints.AddRangeAsync(endpointsToAdd);

        return await _db.SaveChangesAsync();
    }

    private List<ApiEndpoint> BuildEndpoints(
    IReadOnlyList<CsvRowDto> rows,
    Dictionary<string, (Company company, Project project)> cache)
    {
        var endpointsToAdd = new List<ApiEndpoint>();

        foreach (var row in rows)
        {
            var result = GetOrCreateCompanyAndProject(row, cache);
            if (result is null)
                continue;

            var (_, project) = result.Value;

            var endpoint = CreateEndpointIfValid(row, project);
            if (endpoint != null)
                endpointsToAdd.Add(endpoint);
        }

        return endpointsToAdd;
    }

    private (Company company, Project project)? GetOrCreateCompanyAndProject(
    CsvRowDto row,
    Dictionary<string, (Company, Project)> cache)
    {
        var result = _companyFactory.CreateFromRow(row, out var key);
        if (result is null)
            return null;

        var (companyFromFactory, projectFromFactory) = result.Value;

        // 1️⃣ cache (ten sam import)
        if (cache.TryGetValue(key, out var cached))
            return cached;

        // 2️⃣ baza – COMPANY
        var existingCompany = _db.Companies
            .Include(c => c.Projects)
            .FirstOrDefault(c =>
                c.Name == companyFromFactory.Name &&
                c.Initials == companyFromFactory.Initials);

        if (existingCompany is not null)
        {
            // 2a️⃣ PROJECT w tej firmie
            var existingProject = existingCompany.Projects
                .FirstOrDefault(p => p.Name == projectFromFactory.Name);

            if (existingProject is not null)
            {
                cache[key] = (existingCompany, existingProject);
                return (existingCompany, existingProject);
            }

            // 2b️⃣ nowy project do istniejącej firmy
            projectFromFactory.Company = existingCompany;
            _db.Projects.Add(projectFromFactory);

            cache[key] = (existingCompany, projectFromFactory);
            return (existingCompany, projectFromFactory);
        }

        // 3️⃣ nowa firma + projekt
        _db.Companies.Add(companyFromFactory);
        _db.Projects.Add(projectFromFactory);

        cache[key] = (companyFromFactory, projectFromFactory);
        return (companyFromFactory, projectFromFactory);
    }

    private ApiEndpoint? CreateEndpointIfValid(CsvRowDto row, Project project)
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
            Project = project
        };
    }
}