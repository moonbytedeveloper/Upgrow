using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public class MasterProgrammingLanguageCommand : IMasterCommand
    {
        public string? UUID { get; set; } 
        public string? Title { get; set; } 
        public bool IsActive { get; set; }
    }
}
