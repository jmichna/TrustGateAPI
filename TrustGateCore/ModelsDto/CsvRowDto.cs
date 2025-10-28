using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto;
public record CsvRowDto(Dictionary<string, string> Columns);
