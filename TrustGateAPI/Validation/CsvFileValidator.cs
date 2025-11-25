namespace TrustGateAPI.Validation;

public static class CsvFileValidator
{
    public static void ValidateImportFile(IFormFile file)
    {

        if (file.Length == 0)
            throw new ArgumentException("File is empty.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only CSV files are allowed.");
    }
}