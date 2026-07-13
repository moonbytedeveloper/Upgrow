using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Website_MasterIndustry : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public int Sequence { get; set; }

    }
}