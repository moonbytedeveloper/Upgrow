using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterBannerDto
    {
        public string UUID { get; set; } = null!;
        public string TenantUUID { get; set; } = null!;
        public string MainTitle { get; set; } = null!;
        public string SubTitle { get; set; } = null!;
        public string OptionalTitle { get; set; } 
        public string ButtonText { get; set; } = null!;
        public string ButtonURL { get; set; } = null!;
        public string? BannerImage { get; set; } 
        public decimal? SequenceNo { get; set; } = null!;
        public bool IsActive { get; set; }
        
    }
}
