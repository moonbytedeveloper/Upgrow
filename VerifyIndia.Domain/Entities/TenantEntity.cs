using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Common;

namespace Upgrow.Domain.Entities
{
    public abstract class TenantEntity : IMasterEntity, ITenantEntity
    {
        public decimal Id { get; set; }
        public string UUID { get; set; }
        public bool IsActive { get; set; }

        public int TenantId { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
