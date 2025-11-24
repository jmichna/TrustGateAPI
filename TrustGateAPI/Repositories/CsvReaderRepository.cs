using TrustGateAPI.Services.Interfaces;
using TrustGateCore.ModelsDto;

namespace TrustGateAPI.Repositories;

public class CsvReaderRepository : ICsvReaderService
{
    public async Task<IReadOnlyList<CsvRowDto>> ReadAsync(IFormFile file)
    {
        ValidateFile(file);

        await using var stream = file.OpenReadStream();
        return await ParseCsv(stream);
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file is null)
            throw new ArgumentException("No file found.");

        if (file.Length == 0)
            throw new ArgumentException("File is empty.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Only .csv files are allowed");
    }

    private async Task<IReadOnlyList<CsvRowDto>> ParseCsv(Stream csvStream)
    {
        var content = await ReadContent(csvStream);
        var lines = SplitLines(content);

        if (lines.Count == 0)
            return Array.Empty<CsvRowDto>();

        var headers = SplitCsvLine(lines[0]);
        return ParseRows(lines.Skip(1), headers);
    }

    private async Task<string> ReadContent(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    private List<string> SplitLines(string content)
    {
        return content
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    private IReadOnlyList<CsvRowDto> ParseRows(IEnumerable<string> lines, List<string> headers)
    {
        var result = new List<CsvRowDto>();

        foreach (var line in lines)
        {
            var rowDict = ParseRow(line, headers);
            result.Add(new CsvRowDto(rowDict));
        }

        return result;
    }

    private Dictionary<string, string> ParseRow(string line, List<string> headers)
    {
        var cells = SplitCsvLine(line);
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (int c = 0; c < headers.Count; c++)
        {
            dict[headers[c]] = c < cells.Count ? cells[c] : string.Empty;
        }

        return dict;
    }

    private static List<string> SplitCsvLine(string line)
    {
        var values = new List<string>();
        var sb = new System.Text.StringBuilder();
        bool insideQuotes = false;

        foreach (var ch in line)
        {
            if (ch == '"')
            {
                insideQuotes = !insideQuotes;
                continue;
            }

            if (ch == ',' && !insideQuotes)
            {
                values.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(ch);
            }
        }

        values.Add(sb.ToString());
        return values;
    }
}