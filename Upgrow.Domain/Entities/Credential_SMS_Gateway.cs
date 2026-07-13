using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Domain.Entities
{
    public class Credential_SMS_Gateway : BaseEntity
    {
        public string APIKey { get; set; }
        public string SenderId { get; set; }
    }
}
