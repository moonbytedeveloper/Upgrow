using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Auth
{
    public class ApiLoginRequestDto
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string ChannelType { get; set; } = string.Empty;        // SMS | WhatsApp

    }
}
