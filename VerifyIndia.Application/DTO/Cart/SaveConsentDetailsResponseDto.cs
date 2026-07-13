using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Cart
{
    public class SaveConsentDetailsResponseDto
    {
        public bool IsConsentProvided { get; set; }

        public string AuthFor { get; set; }
            = string.Empty;
    }
}
