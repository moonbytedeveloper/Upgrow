using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class SaveBasicInfoRequestDto
    {

        public string Email { get; set; }
            = string.Empty;

        public string StateUUID { get; set; }
            = string.Empty;

        public string CityUUID { get; set; }
            = string.Empty;

        public string IndustryUUID { get; set; }
            = string.Empty;
    }
}
