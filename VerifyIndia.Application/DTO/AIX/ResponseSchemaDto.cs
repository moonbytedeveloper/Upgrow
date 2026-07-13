using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class ResponseSchemaDto
    {
        public string? UUID { get; set; }
        public string? ApiXVersionUUID { get; set; }
        public string? ShortDescription { get; set; }
        public string? SchemaType { get; set; }
        public bool IsSchemaForSuccess { get; set; }
        public bool IsActive { get; set; }
    }
}
