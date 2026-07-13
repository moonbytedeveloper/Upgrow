using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class ApiInfoSection : BaseEntity
    {
         
        public string ApiUUID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
    }
}
