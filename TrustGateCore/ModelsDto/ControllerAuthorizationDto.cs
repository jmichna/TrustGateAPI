using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.ModelsDto
{
    public class ControllerAuthorizationDto
    {
        public int Id { get; protected set; }
        public string? ControllerName { get; protected set; }
        public bool Generic { get; protected set; }

        public int UserId { get; protected set; }
        public int CompanyId { get; protected set; }
    }
}
