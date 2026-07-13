using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.WL
{
    public class WLMasterCMSDto
    {
        public string UUID { get; set; } 
        public string? PageTitle { get; set; } 
        public string? UploadImage { get; set; }
        public string? Description { get; set; } 
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
    }
}
