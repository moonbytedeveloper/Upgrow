using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterDosDontsDto
    {      
        public string UUID { get; set; }
        public string Message { get; set; }
        public bool IsDos { get; set; }
        public decimal SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
    public class GetDosDontsDto
    {
        public string Message { get; set; }       
        public decimal SequenceNo { get; set; }
    }

  
}
