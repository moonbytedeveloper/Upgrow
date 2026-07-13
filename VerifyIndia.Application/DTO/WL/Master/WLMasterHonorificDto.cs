using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.WL.Master
{
    public class WLMasterHonorificDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
