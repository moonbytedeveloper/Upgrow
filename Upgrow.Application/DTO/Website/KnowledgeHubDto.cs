using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Website
{
    public class KnowledgeHubDto
    {
        public string? UUID { get; set; }
        public string? CategoryUUID { get; set; }
        public string? ImageURL { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
