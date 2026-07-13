using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.DTO.Master
{
    public class MasterPolicyDto
    {
        public string UUID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string PolicyContent { get; set; } = null!;
        public string? Version { get; set; }
        public string Code { get; set; } = null!;
        public int SequenceNo { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
