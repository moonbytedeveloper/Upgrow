using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class ApiXCodeMapperDto
    {
        public string? UUID { get; set; }
        public string? ApiXVersionUUID { get; set; }
        public string? ResponseSchemaUUID { get; set; }
        public string? StatusUUID { get; set; }
        public string? CodeExampleUUID { get; set; }
        public string? ResponseJson { get; set; }
        public string? ShortDescription { get; set; }
        public bool IsActive { get; set; }
    }
}