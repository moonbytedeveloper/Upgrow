using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Master_Document : BaseEntity
    {
        public string? Title { get; set; }
        public string? FileType { get; set; }
        public string? Path { get; set; }
       
    }
}
