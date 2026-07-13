using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Api
{
    public class ApiEndpointDto
    {
        public string UUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public string EndpointUrl { get; set; } = null!;
        public bool IsActive { get; set; }
        public string HttpMethod { get; set; } = null!;

        public int Sequence { get; set; }
    }
}
