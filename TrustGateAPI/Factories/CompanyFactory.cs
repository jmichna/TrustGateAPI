using TrustGateAPI.Factories.Interfaces;
using TrustGateCore.Models;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Factories
{
    public class CompanyFactory : ICompanyFactory
    {
        public (Company company, Project project)? CreateFromRow(
            CsvRowDto row,
            out string cacheKey)
        {
            cacheKey = string.Empty;

            row.Columns.TryGetValue("NazwaFirmy", out var companyName);
            row.Columns.TryGetValue("InicjalyFirmy", out var companyInitials);
            row.Columns.TryGetValue("NazwaProjektu", out var projectName);

            if (string.IsNullOrWhiteSpace(companyName))
                return null;

            var companyNameNorm = companyName.Trim();
            var initialsNorm = (companyInitials ?? string.Empty).Trim();
            var projectNameNorm = string.IsNullOrWhiteSpace(projectName)
                ? "Default"
                : projectName.Trim();

            cacheKey =
                $"{companyNameNorm.ToUpperInvariant()}|" +
                $"{initialsNorm.ToUpperInvariant()}|" +
                $"{projectNameNorm.ToUpperInvariant()}";

            var company = new Company
            {
                Name = companyNameNorm,
                Initials = initialsNorm
            };

            var project = new Project
            {
                Name = projectNameNorm,
                Company = company
            };

            return (company, project);
        }
    }
}
