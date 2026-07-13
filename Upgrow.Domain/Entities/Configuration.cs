using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Configuration
    {
        public decimal Id { get; set; }
        public string? DomainUrl { get; set; }
        public string? SandboxAccessToken { get; set; }
    }
}
