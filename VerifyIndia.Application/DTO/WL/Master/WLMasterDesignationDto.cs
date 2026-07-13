using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.WL.Master
{
    public class WLMasterDesignationDto
    {
        public string? UUID { get; set; }
        public string? Title { get; set; }
        public string? ShortTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
