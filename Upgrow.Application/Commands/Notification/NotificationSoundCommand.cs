using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.Notification
{
    public class NotificationSoundCommand :IMasterCommand
    {
        public string? UUID { get; set; }
        public string? SoundName { get; set; }
        public decimal? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
