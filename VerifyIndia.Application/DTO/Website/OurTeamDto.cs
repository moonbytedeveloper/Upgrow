using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Website
{
    public class OurTeamDto
    {
        public string? UUID { get; set; }
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? ImageFile { get; set; }
        public string? IconURL { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
