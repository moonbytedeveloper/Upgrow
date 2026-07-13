using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Website
{
    public class CertificationsDto
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? FilePath { get; set; }
        public bool IsActive { get; set; }
    }
}
