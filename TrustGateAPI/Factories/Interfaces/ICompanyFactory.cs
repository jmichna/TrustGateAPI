using TrustGateCore.Models;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Factories.Interfaces
{
    public interface ICompanyFactory
    {
        (Company company, Project project)? CreateFromRow(
            CsvRowDto row,
            out string cacheKey);
    }
}
