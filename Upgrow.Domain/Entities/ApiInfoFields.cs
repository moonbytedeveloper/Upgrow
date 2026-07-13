using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class ApiInfoFields : BaseEntity
    {

        public string InfoSectionUUID { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
    }
}
