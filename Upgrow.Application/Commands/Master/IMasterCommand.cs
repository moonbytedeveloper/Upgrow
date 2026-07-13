using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Commands.Master
{
    public interface IMasterCommand
    {
        string? UUID { get; set; }
    }
}
