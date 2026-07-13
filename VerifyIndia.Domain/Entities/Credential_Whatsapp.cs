using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Domain.Entities
{
    public class Credential_Whatsapp : BaseEntity
    {
        public string APIKey { get; set; }
        public string AccessToken { get; set; }
        public string MobileNo { get; set; }
        public string SenderName { get; set; }
    }
}
