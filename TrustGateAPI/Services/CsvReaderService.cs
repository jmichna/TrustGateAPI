using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Services;

public class CsvReaderService(ICsvReaderService repository)
    : ICsvReaderService
{
    public Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file)
        => repository.ReadAsync(file);
}
