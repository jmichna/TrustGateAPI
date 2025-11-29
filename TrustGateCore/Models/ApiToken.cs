using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;
public class ApiToken
{
    public int Id { get; set; }

    public string Value { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    public string Status { get; set; } = "Generated";

    public ICollection<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
}
