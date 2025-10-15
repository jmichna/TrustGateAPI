using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrustGateCore.Models
{
    public class ControllerUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public ICollection<ControllerAuthorization> Authorizations { get; set; } = new List<ControllerAuthorization>();
    }
}
