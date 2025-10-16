using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto;

public class CompanyDto
{
    public int Id { get; protected set; }
    public string CompanyName { get; protected set; } = string.Empty;
    public string CompanyInitials { get; protected set; } = string.Empty;
    public string ProjectName { get; protected set; } = string.Empty;
    public int ProjectId { get; protected set; }
}
