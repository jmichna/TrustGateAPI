using TrustGateAPI.Repositories.Interfaces;
using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Services;

public class CsvReaderService(ICsvReaderRepository repository)
    : ICsvReaderService
{
    public Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file)
        => repository.ReadAsync(file);
}
