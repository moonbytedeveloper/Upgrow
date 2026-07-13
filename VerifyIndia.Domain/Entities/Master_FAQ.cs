using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Domain.Entities
{
    public class Master_FAQ : IMasterEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? FAQCategoryUUID { get; set; }
        public string? FAQSubCategoryUUID { get; set; }
        public bool IsActive { get; set; }
    }
}
