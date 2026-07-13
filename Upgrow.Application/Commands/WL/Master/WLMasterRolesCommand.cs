using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands.WL.Master
{
    public class WLMasterRolesCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string Title { get; set; } = null!;
        public string ShortTitle { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
