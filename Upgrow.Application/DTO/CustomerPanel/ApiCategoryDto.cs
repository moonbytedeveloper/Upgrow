using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.CustomerPanel
{
    public class ApiCategoryDto
    {
        public string UUID { get; set; } = default!;
        public string CategoryName { get; set; } = default!;
        public decimal SequenceNo { get; set; }
        public string Icon { get; set; }
        public int ApiCount { get; set; } = 7;

    }
}
