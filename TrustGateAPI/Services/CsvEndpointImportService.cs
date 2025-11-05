using Microsoft.EntityFrameworkCore;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.Models;
using TrustGateSqlLiteService.Db;

namespace TrustGateAPI.Services;

public class CsvEndpointImportService : ICsvEndpointImportService
{
    private readonly SqlDbContext _db;
    private readonly ICsvReaderService _csvReader;

    public CsvEndpointImportService(SqlDbContext db, ICsvReaderService csvReader)
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

        // 1) wczytaj CSV -> list<CsvRowDto>
        await using var stream = file.OpenReadStream();
        var rows = await _csvReader.ReadAsync(stream);

        var endpointsToAdd = new List<ApiEndpoint>();

        foreach (var row in rows)
        {
            // 2) wyciągamy dane z wiersza
            // używamy TryGetValue żeby nie wywaliło się na brakującej kolumnie
            row.Columns.TryGetValue("NazwaFirmy", out var companyName);
            row.Columns.TryGetValue("InicjalyFirmy", out var companyInitials);
            row.Columns.TryGetValue("NazwaProjektu", out var projectName);
            row.Columns.TryGetValue("IdProjektu", out var projectIdString);
            row.Columns.TryGetValue("EndpointName", out var endpointName);
            row.Columns.TryGetValue("HttpMethod", out var httpMethod);
            row.Columns.TryGetValue("Route", out var route);

            if (string.IsNullOrWhiteSpace(companyName))
                continue; // bez nazwy firmy nie zapisujemy

            // 3) upewniamy się, że mamy ProjectId
            int projectId = 0;
            if (!string.IsNullOrWhiteSpace(projectIdString))
                int.TryParse(projectIdString, out projectId);

            // 4) sprawdzamy czy firma już istnieje
            // przyjmijmy unikalność: NazwaFirmy + InicjalyFirmy + ProjectId
            var company = await _db.Companies
                .FirstOrDefaultAsync(c =>
                        c.CompanyName == companyName &&
                        c.CompanyInitials == (companyInitials ?? "") &&
                        c.ProjectId == projectId
                    );

            // 5) jeśli nie ma – tworzymy nową
            if (company is null)
            {
                company = new Company
                {
                    CompanyName = companyName,
                    CompanyInitials = companyInitials ?? string.Empty,
                    ProjectName = projectName ?? string.Empty,
                    ProjectId = projectId
                };

                _db.Companies.Add(company);
                // nie zapisujemy jeszcze – zrobimy na końcu
            }

            // 6) teraz endpoint powiązany z firmą
            // zakładamy, że w CSV są realne dane
            if (!string.IsNullOrWhiteSpace(endpointName) &&
                !string.IsNullOrWhiteSpace(route))
            {
                var endpoint = new ApiEndpoint
                {
                    Name = endpointName,
                    HttpMethod = string.IsNullOrWhiteSpace(httpMethod) ? "GET" : httpMethod,
                    Route = route,
                    Company = company   // EF sam ustawi CompanyId po SaveChanges
                };

                endpointsToAdd.Add(endpoint);
            }
        }

        // 7) dodajemy wszystkie endpointy
        if (endpointsToAdd.Count > 0)
            await _db.ApiEndpoints.AddRangeAsync(endpointsToAdd);

        // 8) zapis do bazy
        var saved = await _db.SaveChangesAsync();
        return saved;
    }
}
