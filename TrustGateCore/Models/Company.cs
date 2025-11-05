using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;

public class Company
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyInitials { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int ProjectId { get; set; }

    public ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();
    public ICollection<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
}

