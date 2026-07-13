using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Common.Dropdowns
{
    public sealed class DropdownDefinition
    {
        public string Key { get; init; } = default!;
        public Type EntityType { get; init; } = default!;
        public IReadOnlyList<FilterRule> Filters { get; init; } = [];
        public bool ApplyActiveFilter { get; init; } = true;
        public bool ApplyDisplayFilter { get; init; } = true;
    }
}
