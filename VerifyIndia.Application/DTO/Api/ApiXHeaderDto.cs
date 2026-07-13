using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Api
{
    public class ApiXHeaderDto
    {
        public string UUID { get; set; }
        public string FieldName { get; set; }
        public string FieldDetails { get; set; }
        public string DataType { get; set; }
        public bool IsRequired { get; set; }
    }
}
