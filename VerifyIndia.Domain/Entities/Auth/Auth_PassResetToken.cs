using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities.Auth
{
    public class Auth_PassResetToken
    {
        public decimal Id { get; set; }
        public string EmployeeUUID { get; set; }
        public string Token { get; set; }
        public DateTime? Expiry { get; set; }
        public bool? IsUsed { get; set; }
        public byte[]? RecordHash { get; set; }
    }
}
