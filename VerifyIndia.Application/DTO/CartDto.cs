using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO
{
    public class CartDto
    {
        public string UUID { get; set; } = null!;
        public string ApiUUID { get; set; } = null!;
        public string CustomerUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
