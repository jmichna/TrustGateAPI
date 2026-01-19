using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models;

public class ApiEndpointToken
{
    public int ApiEndpointId { get; set; }
    public ApiEndpoint ApiEndpoint { get; set; } = null!;

    public int ApiTokenId { get; set; }
    public ApiToken ApiToken { get; set; } = null!;
}