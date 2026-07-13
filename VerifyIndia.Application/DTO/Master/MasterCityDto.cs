using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterCityDto
    {
        public string UUID { get; set; } = null!;
        public string CountryUUID { get; set; } = null!;        
        public string StateUUID { get; set; } = null!;       
        public string Title { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
