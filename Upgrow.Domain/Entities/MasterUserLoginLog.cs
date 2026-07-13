using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class MasterUserLoginLog
    {
        public int Id { get; set; }
        public string? UUID { get; set; } = Guid.NewGuid().ToString();
        public string? EmployeeUUID { get; set; }
        public string? IPAddress { get; set; }
        public DateTime LoginAt { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
