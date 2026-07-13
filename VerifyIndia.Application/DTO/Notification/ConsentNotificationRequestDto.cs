using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Notification
{
    public class ConsentNotificationRequestDto
    {
        public string MobileNo { get; set; }
            = string.Empty;

        public string ConsentUrl { get; set; }
            = string.Empty;
    }
}
