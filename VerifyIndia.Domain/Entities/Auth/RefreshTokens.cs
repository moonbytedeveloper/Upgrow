using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities.Auth
{
    public class RefreshTokens
    {
        public decimal Id { get; set; }
        public string? CustomerUUID { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset? ExpiryAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public bool IsRevoked { get; set; }

        public byte[]? RecordHash { get; set; }
    }
}
