using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Repositories.Interfaces;

public interface ICsvReaderRepository
{
    Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file);
}