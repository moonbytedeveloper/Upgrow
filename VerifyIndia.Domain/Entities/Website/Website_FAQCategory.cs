using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities.Website
{
    public class Website_FAQCategory : BaseEntity
    {
 
        public string Title { get; set; }
        public string? Icon { get; set; }
        public string? ShortDescription { get; set; }
 
    }
}
