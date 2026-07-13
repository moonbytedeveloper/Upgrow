using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class WL_MasterTestimonial : TenantEntity
    {
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Comment { get; set; }
        public string? FilePath { get; set; }
        public decimal? Star { get; set; }
        public decimal? SequenceNo { get; set; }
     
        [NotMapped]
        public string? TenantName { get; set; }
    }
}
