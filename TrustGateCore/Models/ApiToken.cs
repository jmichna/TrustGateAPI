using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;

public class ApiToken
{
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; } = true;

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public ICollection<ApiEndpointToken> ApiEndpointTokens { get; set; }
        = new List<ApiEndpointToken>();
}
