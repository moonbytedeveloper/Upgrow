using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class LocationLookupResponseDto
    {
        public string? StateUUID { get; set; }

        public string? CityUUID { get; set; }
    }
}
