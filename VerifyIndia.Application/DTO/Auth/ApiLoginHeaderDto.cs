using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Auth
{
    public class ApiLoginHeaderDto
    {
      
        [FromHeader(Name = "X-Tenant")]
        public string TenantIdentifier { get; set; } = string.Empty;
        [FromHeader(Name = "X-LatLong")]
        public string LatLong { get; set; } = string.Empty;
        [FromHeader(Name = "X-IpAddress")]
        public string IpAddress { get; set; } = string.Empty;
        [FromHeader(Name = "X-Timestamp")]
        public string Timestamp { get; set; } = string.Empty;
        [FromHeader(Name = "X-Signature")]
        public string Signature { get; set; } = string.Empty;

    }
}
