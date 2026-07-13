using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.WL
{
    public class WLTestimonialDto
    {
        public string? UUID { get; set; }
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Comment { get; set; }
        public string? FilePath { get; set; }
        public decimal? Star { get; set; }
        public decimal? SequenceNo { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }
        public string? TenantName { get; set; }
    }
}
