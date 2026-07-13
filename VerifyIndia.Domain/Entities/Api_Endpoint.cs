using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Api_Endpoint : BaseEntity
    {
        public string? ApiUUID { get; set; }
        public string? EndpointUrl { get; set; }
        public string? HttpMethod { get; set; }
        public string? VerificationCode { get; set; }
        public int Sequence { get; set; }
    }
}
