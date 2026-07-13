using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterCareerDto
    {
        public string? UUID { get; set; }
        public string? DepartmentUUID { get; set; }
        public string? CountryUUID { get; set; }
        public string? StateUUID { get; set; }
        public string? CityUUID { get; set; }
        public string? Name { get; set; }
        public string? IconImage { get; set; }
        public string? Experience { get; set; }
        public int? NumberOfPosition { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? SkillsUUID { get; set; }
        public string? JobTypeUUID { get; set; }
        public bool IsActive { get; set; }

    }
}
