using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Upgrow.Application.DTO.Verification.Common
{
    public class BaseDto
    {
        [JsonIgnore]
        public string refid { get; set; }

        protected BaseDto()
        {
            refid = Utils.GetUUID();
        }
    }
}
