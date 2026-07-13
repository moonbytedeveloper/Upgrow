using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class ApixHeaderDto
    {
        public string? UUID { get; set; }
        public string? ApiXVersionUUID { get; set; }
        public string? FieldName { get; set; }
        public string? DataType { get; set; }
        public string? FieldDetails { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
    }
}
