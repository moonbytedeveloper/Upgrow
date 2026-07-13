using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyIndia.Application.Common.Results
{
    public sealed record Error(
    string Code,
    string Message,
    string? Field = null);
}
