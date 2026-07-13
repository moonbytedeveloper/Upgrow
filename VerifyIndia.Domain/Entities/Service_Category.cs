using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.Auth
{
    public class Service_Category : BaseEntity
    {
 
        public string CategoryName { get; set; }
        public string IconImage { get; set; }
        public string Description { get; set; }
 
    }
}
