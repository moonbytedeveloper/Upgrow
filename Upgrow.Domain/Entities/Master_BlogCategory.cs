using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;

namespace Upgrow.Domain.Entities
{
    public class Master_BlogCategory : BaseEntity
    {
        public string? Title { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }      
    }
}
