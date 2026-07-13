using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Configuration
    {
        public decimal Id { get; set; }
        public string? DomainUrl { get; set; }
        public string? SandboxAccessToken { get; set; }
    }
}
