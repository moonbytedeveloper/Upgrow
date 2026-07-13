using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Verification.SprintVerify;
using Upgrow.Application.Helper;

namespace Upgrow.Application.IServices.Verification.SprintVerify
{
    public interface IFSSAIVerifyService
    {
        Task<ApiResponse<JsonElement>> GetFSSAIVerify(FSSAIDto request);

    }
}
