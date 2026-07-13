using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class ApiXCodeExample : BaseEntity
    {
        public string StatusCodeUUID { get; set; }
        public string ErrorTitle { get; set; }
        public string Message { get; set; }
        
    }
}