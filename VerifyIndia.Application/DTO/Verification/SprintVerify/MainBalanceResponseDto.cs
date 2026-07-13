using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Verification.SprintVerify
{
    public class MainBalanceResponseDto
    {
        public int statuscode { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public decimal data { get; set; }
    }
}
