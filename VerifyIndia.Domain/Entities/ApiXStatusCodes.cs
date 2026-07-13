using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXStatusCodes : IMasterEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public string Title {get; set;}
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsActive { get; set; }
    }
}
