using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.Website
{
    public class Website_MasterFAQ : BaseEntity
    {
 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FAQCategoryUUID { get; set; }
 
        public bool IsFeatured { get; set; }
    }
}
