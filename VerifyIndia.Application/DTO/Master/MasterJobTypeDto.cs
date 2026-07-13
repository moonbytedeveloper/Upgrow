using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterJobTypeDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
