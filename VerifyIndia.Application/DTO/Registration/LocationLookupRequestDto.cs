using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class LocationLookupRequestDto
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
