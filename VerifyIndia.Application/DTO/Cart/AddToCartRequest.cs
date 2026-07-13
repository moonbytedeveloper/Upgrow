using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Cart
{
    public class AddToCartRequest
    {
        public string ApiUUID { get; set; }
            = string.Empty;

        /// <summary>
        /// Required only while creating first cart.
        /// Self / Other
        /// </summary>
        public string? AuthFor { get; set; }

        public string? ReqPayload { get; set; }
    }
}
