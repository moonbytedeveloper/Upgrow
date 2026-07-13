using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterFaqDto
    {
        public string? UUID { get; set; }
        public string FAQCategoryUUID { get; set; }

        public string FAQSubCategoryUUID { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
