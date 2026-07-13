using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Verification
{
    public class ApiExecutionStep
    {
        public string VerificationCode { get; set; }
            = string.Empty;

        public int Sequence { get; set; }

        public string EndpointUrl { get; set; }
            = string.Empty;

        public string HttpMethod { get; set; }
            = string.Empty;
    }
}
