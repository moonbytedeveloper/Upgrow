using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class CredentialsSMSGatewayDto
    {
        public string? UUID { get; set; }
        public string APIKey { get; set; }
        public string SenderId { get; set; }
        public bool IsActive { get; set; }
    }
}
