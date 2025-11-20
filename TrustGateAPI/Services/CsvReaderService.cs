using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Services
{
    public class CsvReaderService : ICsvReaderService
    {
        private readonly ICsvReaderService _repository;

        public CsvReaderService(ICsvReaderService repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file)
            => _repository.ReadAsync(file);
    }
}
