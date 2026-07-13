using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Banner : BaseEntity
    {
        
        public string? MainTitle { get; set; }
        public string? SubTitle { get; set; }
        public string? OptionalTitle { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonURL { get; set; }
        public string? BannerImage { get; set; }
        public decimal SequenceNo { get; set; }
    }
}
