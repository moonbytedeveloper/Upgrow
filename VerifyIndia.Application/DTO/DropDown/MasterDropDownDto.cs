using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.DropDown
{
    public class MasterDropDownDto
    {
        public string ForeignKey { get; set; } = default!;
        public string UUID { get; set; } = default!;
        public string Title { get; set; } = default!;
    }
}
