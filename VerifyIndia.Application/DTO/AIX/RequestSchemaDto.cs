using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class RequestSchemaDto
    {
        public string? UUID { get; set; }
        public string? ApiXVersionUUID { get; set; }
        public string? RequestType { get; set; }
        public string? RequestJson { get; set; }
        public bool IsActive { get; set; }
    }
}
