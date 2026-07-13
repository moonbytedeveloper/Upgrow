using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;

namespace Upgrow.Application.Commands
{
    public class PinnedServiceCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string? ApiUUID { get; set; } = null!;
        public string? CustomerUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
