using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class WLMasterCityDto
    {
        public string UUID { get; set; } = null!;
        public string CountryUUID { get; set; } = null!;
        public string? CountryName { get; set; }
        public string StateUUID { get; set; } = null!;
        public string? StateName { get; set; }
        public string Title { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
