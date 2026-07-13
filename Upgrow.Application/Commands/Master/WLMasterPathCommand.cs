using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class WLMasterPathCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
