using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Repositories;

public class CsvReaderRepository : ICsvReaderService
{
    public async Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file)
    {
        if (file is null)
            throw new ArgumentException("Plik jest wymagany.");

        if (file.Length == 0)
            throw new ArgumentException("Plik jest pusty.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Dozwolone są tylko pliki .csv.");

        await using var stream = file.OpenReadStream();
        return await ParseAsync(stream);
    }

    private async Task<IReadOnlyList<CsvRowDto>> ParseAsync(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        var content = await reader.ReadToEndAsync();

        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0)
            return Array.Empty<CsvRowDto>();

        var headers = SplitCsvLine(lines[0]);
        var result = new List<CsvRowDto>();

        for (int i = 1; i < lines.Length; i++)
        {
            var cells = SplitCsvLine(lines[i]);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int c = 0; c < headers.Count; c++)
                dict[headers[c]] = c < cells.Count ? cells[c] : string.Empty;

            result.Add(new CsvRowDto(dict));
        }

        return result;
    }

    private static List<string> SplitCsvLine(string line)
    {
        var res = new List<string>();
        var sb = new System.Text.StringBuilder();
        bool quote = false;

        foreach (var ch in line)
        {
            if (ch == '"')
            {
                quote = !quote;
                continue;
            }

            if (ch == ',' && !quote)
            {
                res.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(ch);
            }
        }

        res.Add(sb.ToString());
        return res;
    }
}
