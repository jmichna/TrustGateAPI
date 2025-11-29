using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using TrustGateAPI.Repositories;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateCore.Models;
using TrustGateCore.ModelsDto;
using TrustGateSqlLiteService.Db;

namespace TrustGateTests.TrustGateAPI.Repositories;

[TestFixture]
public class CsvEndpointImportRepositoryTests
{
    private DbContextOptions<SqlDbContext> CreateOptions()
        => new DbContextOptionsBuilder<SqlDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

    private static IFormFile CreateValidCsvFormFile()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.csv");
        fileMock.Setup(f => f.Length).Returns(10);
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3 }));
        fileMock.Setup(f => f.ContentType).Returns("text/csv");
        return fileMock.Object;
    }

    [Test]
    public async Task ImportCompaniesWithEndpointsAsync_ValidRows_AddsCompaniesAndEndpoints()
    {
        // arrange
        var options = CreateOptions();
        await using var db = new SqlDbContext(options);

        var csvReaderMock = new Mock<ICsvReaderRepository>();

        var rows = new List<CsvRowDto>
        {
            new CsvRowDto(
                new Dictionary<string, string>
                {
                    ["NazwaFirmy"] = "Firma X",
                    ["InicjalyFirmy"] = "FX",
                    ["NazwaProjektu"] = "Projekt A",
                    ["IdProjektu"] = "101",
                    ["EndpointName"] = "GetUser",
                    ["HttpMethod"] = "get",
                    ["Route"] = "/users",
                    ["ApiTokenId"] = "5"
                }),
            new CsvRowDto(
                new Dictionary<string, string>
                {
                    ["NazwaFirmy"] = "Firma X",
                    ["InicjalyFirmy"] = "FX",
                    ["NazwaProjektu"] = "Projekt A",
                    ["IdProjektu"] = "101",
                    ["EndpointName"] = "CreateUser",
                    ["HttpMethod"] = "",
                    ["Route"] = "/users/create",
                    ["ApiTokenId"] = ""
                })
        };

        csvReaderMock
            .Setup(r => r.ReadAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(rows);

        var repo = new CsvEndpointImportRepository(db, csvReaderMock.Object);

        var file = CreateValidCsvFormFile();

        // act
        var savedCount = await repo.ImportCompaniesWithEndpointsAsync(file);

        // assert
        var companies = await db.Companies.ToListAsync();
        var endpoints = await db.ApiEndpoints
            .Include(e => e.Company)
            .ToListAsync();

        Assert.That(savedCount, Is.EqualTo(3));
        Assert.That(companies.Count, Is.EqualTo(1));
        Assert.That(endpoints.Count, Is.EqualTo(2));

        Assert.That(endpoints.All(e => e.CompanyId == companies[0].Id), Is.True);

        var createEndpoint = endpoints.Single(e => e.Name == "CreateUser");
        Assert.That(createEndpoint.HttpMethod, Is.EqualTo("GET"));

        var getEndpoint = endpoints.Single(e => e.Name == "GetUser");
        Assert.That(getEndpoint.ApiTokenId, Is.EqualTo(5));
    }

    [Test]
    public async Task ImportCompaniesWithEndpointsAsync_NoValidRows_ReturnsZeroAndSavesNothing()
    {
        // arrange
        var options = CreateOptions();
        await using var db = new SqlDbContext(options);

        var csvReaderMock = new Mock<ICsvReaderRepository>();

        var rows = new List<CsvRowDto>
        {
            new CsvRowDto(
                new Dictionary<string, string>
                {
                    ["NazwaFirmy"] = "",
                    ["EndpointName"] = "Test",
                    ["Route"] = "/test"
                }),
            new CsvRowDto(
                new Dictionary<string, string>
                {
                    ["NazwaFirmy"] = "Firma Y",
                    ["EndpointName"] = "",
                    ["Route"] = "/test2"
                })
        };

        csvReaderMock
            .Setup(r => r.ReadAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(rows);

        var repo = new CsvEndpointImportRepository(db, csvReaderMock.Object);
        var file = CreateValidCsvFormFile();

        // act
        var savedCount = await repo.ImportCompaniesWithEndpointsAsync(file);

        // assert
        Assert.That(savedCount, Is.EqualTo(0));

        Assert.That(await db.Companies.CountAsync(), Is.EqualTo(0));
        Assert.That(await db.ApiEndpoints.CountAsync(), Is.EqualTo(0));
    }
}