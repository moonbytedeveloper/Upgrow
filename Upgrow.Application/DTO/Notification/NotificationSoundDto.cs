using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.DTO.Notification
{
    public class NotificationSoundDto
    {
        public string? UUID { get; set; }
        public string? SoundName { get; set; }
        public decimal? DisplayOrder { get; set; }
        //public bool IsActive { get; set; }
    }
}
