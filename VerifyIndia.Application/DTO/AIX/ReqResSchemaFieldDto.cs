using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.AIX
{
    public class ReqResSchemaFieldDto
    {
        public string? UUID { get; set; }
        public string? SchemaUUID { get; set; }
        public string? ParentUUID { get; set; }
        public string? FieldName { get; set; }
        public string? DataType { get; set; }
        public string? Description { get; set; }
        public string? Examlpe { get; set; }
        public bool? IsArray { get; set; }
        public int? DisplayOrder { get; set; }
        public bool AllowNull { get; set; }
        public string? Constraints { get; set; }
        public bool IsActive { get; set; }
    }
}
