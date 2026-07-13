using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Clients : BaseEntity
    {
        public string? Name { get; set; }
        public string? IconImage { get; set; }
    }
}

