using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Common.Results
{
    public enum ErrorType
    {
        None = 0,

        Validation = 1,

        Unauthorized = 2,

        Forbidden = 3,

        NotFound = 4,

        Conflict = 5,

        Business = 6,

    }
}
