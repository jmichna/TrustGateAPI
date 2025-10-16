using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;

public class Authorization
{
    public int Id { get; set; }
    public string? ControllerName { get; set; }
    public bool Generic { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = default!;
}
