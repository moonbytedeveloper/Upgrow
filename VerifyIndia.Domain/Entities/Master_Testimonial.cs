using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Master_Testimonial : BaseEntity
    {

        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Comment { get; set; }
        public string? FilePath { get; set; }
        public decimal? Star { get; set; }
        public decimal? SequenceNo { get; set; }

    }
}
