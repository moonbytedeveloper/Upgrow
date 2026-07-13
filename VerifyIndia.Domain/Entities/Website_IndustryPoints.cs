using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Website_IndustryPoints : BaseEntity
    {
        public string Title { get; set; }
        public string IndustryUUID { get; set; }

        public int Sequence { get; set; }

    }
}