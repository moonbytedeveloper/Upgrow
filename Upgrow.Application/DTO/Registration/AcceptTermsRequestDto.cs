using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Registration
{
    public sealed class AcceptTermsRequestDto
    {

        public List<string> PolicyUUIDs { get; set; }
            = [];
    }
}
