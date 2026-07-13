using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Verification.SprintVerify
{
    public class StatusDto<T>
    {
        public int statuscode { get; set; }

        public bool status { get; set; }

        public string message { get; set; }

        public dynamic data { get; set; }
    }
}
