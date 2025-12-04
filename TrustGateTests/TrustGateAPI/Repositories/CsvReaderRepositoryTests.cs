using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TrustGateAPI.Repositories;
using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Validation;
using TrustGateCore.ModelsDto;

namespace TrustGateTests.TrustGateAPI.Repositories;

[TestFixture]
public class CsvReaderRepositoryTests
{
    private Mock<ICsvReaderRepository> _csvReaderMock = null!;
    private CsvReaderRepository _csvReaderRepo = null!;

    [SetUp]
    public void SetUp()
    {
        _csvReaderMock = new Mock<ICsvReaderRepository>();
        _csvReaderRepo = new CsvReaderRepository();
    }

    [Test]
    public async Task ReadAsync_ValidFile_ReturnsListOfRows()
    {
        // arrange
        var csvFileMock = new Mock<IFormFile>();
        var fileContent = "Header1,Header2\nValue1,Value2";
        var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        csvFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        csvFileMock.Setup(f => f.FileName).Returns("test.csv");
        csvFileMock.Setup(f => f.Length).Returns(memoryStream.Length);

        // act
        var result = await _csvReaderRepo.ReadAsync(csvFileMock.Object);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Columns["Header1"], Is.EqualTo("Value1"));
        Assert.That(result[0].Columns["Header2"], Is.EqualTo("Value2"));
    }

    [Test]
    public async Task ReadAsync_EmptyFile_ReturnsEmptyList()
    {
        // arrange
        var csvFileMock = new Mock<IFormFile>();
        var memoryStream = new MemoryStream();  // pusty plik
        csvFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
        csvFileMock.Setup(f => f.FileName).Returns("empty.csv");
        csvFileMock.Setup(f => f.Length).Returns(0);

        var csvReaderRepo = new CsvReaderRepository();

        // act & assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await csvReaderRepo.ReadAsync(csvFileMock.Object));

        Assert.That(ex.Message, Is.EqualTo("File is empty."));
    }


    [Test]
    public void ParseCsvRow_ValidRow_ReturnsDictionary()
    {
        // arrange
        var row = "Value1,Value2";
        var headers = new List<string> { "Header1", "Header2" };

        // act
        var methodInfo = typeof(CsvReaderRepository)
            .GetMethod("ParseCsvRow", BindingFlags.NonPublic | BindingFlags.Instance);

        var result = methodInfo.Invoke(_csvReaderRepo, new object[] { row, headers });

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.GetType().GetProperty("Count")!.GetValue(result), Is.EqualTo(2));
    }


    [Test]
    public void ParseCsvRow_RowWithMissingCells_ReturnsPartialDictionary()
    {
        // arrange
        var row = "Value1";  // tylko jedna komórka
        var headers = new List<string> { "Header1", "Header2" };

        // act - refleksja do wywołania prywatnej metody
        var methodInfo = typeof(CsvReaderRepository)
            .GetMethod("ParseCsvRow", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = (Dictionary<string, string>)methodInfo.Invoke(_csvReaderRepo, new object[] { row, headers });

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));  // Zwróci 2 klucze
        Assert.That(result["Header1"], Is.EqualTo("Value1"));
        Assert.That(result["Header2"], Is.EqualTo(""));  // brak wartości
    }


    [Test]
    public async Task ParseCsvContent_ValidContent_ReturnsRows()
    {
        // arrange
        var csvContent = "Header1,Header2\nValue1,Value2\nValue3,Value4";
        var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));

        // act - refleksja do wywołania prywatnej metody
        var methodInfo = typeof(CsvReaderRepository)
            .GetMethod("ParseCsvContent", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = await (Task<IReadOnlyList<CsvRowDto>>)methodInfo.Invoke(_csvReaderRepo, new object[] { memoryStream });

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));  // Dwa wiersze w pliku
        Assert.That(result[0].Columns["Header1"], Is.EqualTo("Value1"));
        Assert.That(result[1].Columns["Header2"], Is.EqualTo("Value4"));
    }
}