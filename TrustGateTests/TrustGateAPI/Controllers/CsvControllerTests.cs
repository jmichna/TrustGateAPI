using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TrustGateAPI.Controllers;
using TrustGateAPI.Services;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateTests.TrustGateAPI.Controllers;

[TestFixture]
public class CsvControllerTests
{
    private Mock<ICsvReaderService> _csvReaderMock = null!;
    private Mock<ICsvEndpointImportService> _importServiceMock = null!;
    private CsvController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _csvReaderMock = new Mock<ICsvReaderService>();
        _importServiceMock = new Mock<ICsvEndpointImportService>();
        _controller = new CsvController(_csvReaderMock.Object, _importServiceMock.Object);
    }

    [Test]
    public async Task Upload_WhenFileIsValid_ReturnsOkWithRows()
    {
        // arrange
        var fileMock = new Mock<IFormFile>();
        var expectedRows = new List<CsvRowDto>
        {
            new CsvRowDto(new Dictionary<string, string> { { "Id", "1" }, { "CompanyName", "Company1" } }),
            new CsvRowDto(new Dictionary<string, string> { { "Id", "2" }, { "CompanyName", "Company2" } })
        };

        _csvReaderMock
            .Setup(s => s.ReadAsync(fileMock.Object))
            .ReturnsAsync(expectedRows);

        // act
        var result = await _controller.Upload(fileMock.Object);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.Value, Is.EqualTo(expectedRows));

        _csvReaderMock.Verify(s => s.ReadAsync(fileMock.Object), Times.Once);
    }

    [Test]
    public async Task Upload_WhenArgumentExceptionThrown_ReturnsBadRequest()
    {
        // arrange
        var fileMock = new Mock<IFormFile>();

        _csvReaderMock
            .Setup(s => s.ReadAsync(fileMock.Object))
            .ThrowsAsync(new ArgumentException("Invalid file"));

        // act
        var result = await _controller.Upload(fileMock.Object);

        // assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequest = (BadRequestObjectResult)result;

        Assert.That(badRequest.Value, Is.EqualTo("Invalid file"));
    }

    [Test]
    public async Task ImportCompaniesWithEndpoints_WhenFileValid_ReturnsOkWithSavedCount()
    {
        // arrange
        var fileMock = new Mock<IFormFile>();
        var expectedCount = 10;

        _importServiceMock
            .Setup(s => s.ImportCompaniesWithEndpointsAsync(fileMock.Object))
            .ReturnsAsync(expectedCount);

        // act
        var result = await _controller.ImportCompaniesWithEndpoints(fileMock.Object);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.Value, Is.Not.Null);

        var savedObj = ok.Value!.GetType().GetProperty("saved")!.GetValue(ok.Value);
        Assert.That(savedObj, Is.EqualTo(expectedCount));
    }

    [Test]
    public async Task ImportCompaniesWithEndpoints_WhenArgumentExceptionThrown_ReturnsBadRequest()
    {
        // arrange
        var fileMock = new Mock<IFormFile>();

        _importServiceMock
            .Setup(s => s.ImportCompaniesWithEndpointsAsync(fileMock.Object))
            .ThrowsAsync(new ArgumentException("Invalid CSV"));

        // act
        var result = await _controller.ImportCompaniesWithEndpoints(fileMock.Object);

        // assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var bad = (BadRequestObjectResult)result;

        Assert.That(bad.Value, Is.EqualTo("Invalid CSV"));
    }
}