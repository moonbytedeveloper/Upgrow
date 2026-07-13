using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Registration
{
    public sealed class AcceptDosDontsRequestDto
    {
        public string DocumentUUID { get; set; }
            = string.Empty;

        public bool ConsentGiven { get; set; }
    }
}
