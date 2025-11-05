using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Controllers;

[Authorize]
public class CsvController(ICsvReaderService csv, ICsvEndpointImportService importService) : BaseController
{
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CsvRowDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0) return BadRequest("Brak pliku.");
        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Dozwolone tylko pliki .csv.");

        await using var stream = file.OpenReadStream();
        var rows = await csv.ReadAsync(stream);
        return Ok(rows);
    }

    //// 2) Odczyt z istniejącej ścieżki na serwerze
    //[HttpGet("read")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<CsvRowDto>))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Read([FromQuery] string path, CancellationToken ct)
    //{
    //    if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
    //        return BadRequest("Nieprawidłowa ścieżka.");

    //    await using var fs = System.IO.File.OpenRead(path);
    //    var rows = await csv.ReadAsync(fs, ct);
    //    return Ok(rows);
    //}

    [HttpPost("companies-with-endpoints")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportCompaniesWithEndpoints(IFormFile file)
    {
        var count = await importService.ImportCompaniesWithEndpointsAsync(file);
        return Ok(new { saved = count });
    }
}
