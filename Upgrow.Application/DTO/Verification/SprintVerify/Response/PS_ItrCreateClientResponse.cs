using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.Common.Request;

namespace Upgrow.Application.DTO.Verification.SprintVerify.Response
{
    public class PS_ItrCreateClientResponse
    {
        public string? client_id { get; set; }
        public ClientDataResponse? request { get; set; }
    }

    public class ClientDataResponse
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }
}
