using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Repositories;

public class CsvEndpointRepository : ICsvEndpointImportService
{
    private readonly SqlDbContext _db;
    private readonly ICsvReaderService _csvReader;

    public CsvEndpointRepository(SqlDbContext db, ICsvReaderService csvReader)
    {
        _db = db;
        _csvReader = csvReader;
    }

    public async Task<int> ImportCompaniesWithEndpointsAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("Plik jest pusty lub nie został przesłany.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Dozwolone są tylko pliki CSV.");

        var rows = await _csvReader.ReadAsync(file);

        var endpointsToAdd = new List<ApiEndpoint>();

        // cache firm w pamięci, żeby nie tworzyć duplikatów w ramach jednego importu
        var companyCache = new Dictionary<string, Company>();

        foreach (var row in rows)
        {
            row.Columns.TryGetValue("NazwaFirmy", out var companyName);
            row.Columns.TryGetValue("InicjalyFirmy", out var companyInitials);
            row.Columns.TryGetValue("NazwaProjektu", out var projectName);
            row.Columns.TryGetValue("IdProjektu", out var projectIdString);
            row.Columns.TryGetValue("EndpointName", out var endpointName);
            row.Columns.TryGetValue("HttpMethod", out var httpMethod);
            row.Columns.TryGetValue("Route", out var route);

            if (string.IsNullOrWhiteSpace(companyName))
                continue;

            // normalizacja
            var nameNorm = companyName.Trim();
            var initialsNorm = (companyInitials ?? string.Empty).Trim();
            var projectNameNorm = (projectName ?? string.Empty).Trim();

            // klucz unikalności: Firma + Inicjały + Nazwa Projektu
            var key = $"{nameNorm.ToUpperInvariant()}|{initialsNorm.ToUpperInvariant()}|{projectNameNorm.ToUpperInvariant()}";

            // IdProjektu – jak jest, to bierzemy, jak nie ma → 0
            int projectId = 0;
            if (!string.IsNullOrWhiteSpace(projectIdString))
                int.TryParse(projectIdString, out projectId);

            // szukamy najpierw w cache
            if (!companyCache.TryGetValue(key, out var company))
            {
                // jeśli nie ma w cache – sprawdzamy w bazie
                company = await _db.Companies.FirstOrDefaultAsync(c =>
                        c.CompanyName == nameNorm &&
                        c.CompanyInitials == initialsNorm &&
                        c.ProjectName == projectNameNorm);

                // jeśli nadal brak – tworzymy nową firmę
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

                // zapamiętujemy w cache na kolejne wiersze
                companyCache[key] = company;
            }

            // endpoint dla tej firmy
            if (!string.IsNullOrWhiteSpace(endpointName) &&
                !string.IsNullOrWhiteSpace(route))
            {
                var endpoint = new ApiEndpoint
                {
                    Name = endpointName.Trim(),
                    HttpMethod = string.IsNullOrWhiteSpace(httpMethod) ? "GET" : httpMethod.Trim().ToUpperInvariant(),
                    Route = route.Trim(),
                    Company = company      // tu EF sam ustawi CompanyId
                };

                endpointsToAdd.Add(endpoint);
            }
        }

        if (endpointsToAdd.Count > 0)
            await _db.ApiEndpoints.AddRangeAsync(endpointsToAdd);

        var saved = await _db.SaveChangesAsync();
        return saved;
    }
}
