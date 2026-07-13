using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;

namespace VerifyIndia.Application.Commands
{
    public class CartCommand : IMasterCommand
    {
        public string? UUID { get; set; }
        public string? ApiUUID { get; set; } = null!;
        public string? CustomerUUID { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
