using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.WL.Master
{
    public class WL_ActivityLogs
    {
        public decimal Id { get; set; }

        public byte[]? CurrentHash { get; set; }

        public byte[]? PreviousHash { get; set; }

        public byte[]? DigitalSignature { get; set; }

        public string? PayLoad { get; set; }
        public int TenantId { get; set; }
        public bool IsActive { get; set; }

    }
}
