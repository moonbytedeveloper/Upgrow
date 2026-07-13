using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterTestimonialDto
    {
        public string? UUID { get; set; }
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Comment { get; set; }
        public string? FilePath { get; set; }
        public decimal? Star { get; set; }
        public decimal? SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
