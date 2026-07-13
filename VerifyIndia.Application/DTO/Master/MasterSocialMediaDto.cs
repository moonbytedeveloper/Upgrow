using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterSocialMediaDto
    {
        public string UUID { get; set; } = null!;
        public string PlatformName { get; set; } = null!;
        public string ProfileURL { get; set; } = null!;
        public string IconURL { get; set; } = null!;
        public decimal DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
