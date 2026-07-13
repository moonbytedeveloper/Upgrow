using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Common.Dropdowns
{
    public sealed class DropdownRequest
    {
        public string Key { get; init; } = default!;

        // semantic parent values (Country, State, etc.)
        public Dictionary<string, string> Parents { get; init; } = new();
    }
}
