using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public class Master_FAQSubCategory : IMasterEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public string? FAQCategoryUUID { get; set; }

        public string? Image { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
    }
}
