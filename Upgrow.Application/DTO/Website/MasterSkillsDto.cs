using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Website
{
    public class MasterSkillsDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
