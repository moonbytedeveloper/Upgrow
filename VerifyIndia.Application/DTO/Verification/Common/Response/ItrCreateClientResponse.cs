using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.Common.Request;

namespace VerifyIndia.Application.DTO.Verification.Common.Response
{
    public class ItrCreateClientResponse
    {
        public string? client_id { get; set; }
        public ClientData? request { get; set; }
    }

    public class ClientData
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }
}
