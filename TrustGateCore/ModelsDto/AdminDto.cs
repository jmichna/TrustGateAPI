using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto;

public class AdminDto
{
    public int Id { get; protected set; }
    public string Login { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
}
