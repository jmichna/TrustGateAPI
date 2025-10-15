using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto
{
    public class ControllerUserDto
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; } = string.Empty;
        public string Initials { get; protected set; } = string.Empty;
        public IEnumerable<int>? AuthorizationIds { get; protected set; }
    }
}
