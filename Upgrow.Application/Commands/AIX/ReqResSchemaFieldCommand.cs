using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.AIX
{
    public class ReqResSchemaFieldCommand : IMasterCommand
    {
        public string? UUID { get; set; }

        // This will be set server-side to the parent RequestSchema.UUID
        public string? SchemaUUID { get; set; }

        public string? ParentUUID { get; set; }

        [Required(ErrorMessage = "Field Name is required")]
        public string? FieldName { get; set; }

        [Required(ErrorMessage = "Data Type is required")]
        public string? DataType { get; set; }

        public string? Description { get; set; }

        public string? Examlpe { get; set; }

        public bool IsArray { get; set; }

        public int DisplayOrder { get; set; }

        public bool AllowNull { get; set; }

        public string? Constraints { get; set; }

        public bool IsActive { get; set; }
    }
}
