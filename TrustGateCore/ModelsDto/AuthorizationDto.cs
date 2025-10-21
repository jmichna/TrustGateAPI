using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto;

public class AuthorizationDto
{
    public int Id { get; protected set; }

    // Klucze obce – razem
    public int UserId { get; protected set; }
    public int CompanyId { get; protected set; }

    // Właściwości domenowe – razem
    public string? ControllerName { get; protected set; }
    public bool Generic { get; protected set; }
}
