using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;
public class ApiEndpoint
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string HttpMethod { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;

    public int CompanyId { get; set; }

    public Company Company { get; set; } = null!;
}
