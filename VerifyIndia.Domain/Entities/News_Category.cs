using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class News_Category : BaseEntity
    {
 
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
 
    }
}
