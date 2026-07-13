using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Common.Dropdowns
{
    public sealed class FilterRule
    {
        public string ParentKey { get; init; } = default!;     // UI concept
        public string EntityProperty { get; init; } = default!; // DB property
    }
}
