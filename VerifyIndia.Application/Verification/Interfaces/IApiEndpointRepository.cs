using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Verification.Interfaces
{
    public interface IApiEndpointRepository
    {
        Task<List<ApiExecutionStep>>
            GetExecutionStepsAsync(
                string apiUuid);
    }
}
