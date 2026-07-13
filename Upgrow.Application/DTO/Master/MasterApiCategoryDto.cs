using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Master
{
    public class MasterApiCategoryDto
    {
        public string UUID { get; set; } = null!;
        public string CategoryName { get; set; }
        public decimal SequenceNo { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
    }
}
