using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class ApiXCodeExampleDto
    {
        public string? UUID { get; set; }
        public string? StatusCodeUUID { get; set; }
        public string? ErrorTitle { get; set; }
        public string? Message { get; set; }
        public bool IsActive { get; set; }
    }
}
