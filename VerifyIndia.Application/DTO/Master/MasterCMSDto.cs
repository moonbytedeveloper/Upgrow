using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTOs.Master
{
    public class MasterCMSDto
    {
        public string UUID { get; set; } = null!;
        public string PageTitle { get; set; } = null!;
        public string UploadImage { get; set; } = null!;
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
