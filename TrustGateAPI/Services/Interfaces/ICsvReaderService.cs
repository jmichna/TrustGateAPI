using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Services.Interfaces
{
    public interface ICsvReaderService
    {
        Task<IReadOnlyList<CsvRowDto>> ReadAsync(Stream csvStream);
    }
}
