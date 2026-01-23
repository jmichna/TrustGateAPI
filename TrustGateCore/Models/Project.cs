using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ICollection<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
    public ICollection<ApiToken> ApiTokens { get; set; } = new List<ApiToken>();
}
