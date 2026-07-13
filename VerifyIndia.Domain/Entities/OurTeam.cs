using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class OurTeam : BaseEntity
    {
 
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? ImageFile { get; set; }
        public string? Description { get; set; }
        public string? IconURL { get; set; }
 
    }
}
