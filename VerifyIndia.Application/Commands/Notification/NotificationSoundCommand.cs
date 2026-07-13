using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands.Notification
{
    public class NotificationSoundCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        public string? SoundName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
