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
    public interface IRCVerifyService
    {
        public Task<ApiResponse<JsonElement>> GetRCVerify(RCVerifyDto request);
        public Task<ApiResponse<JsonElement>> GetRCAdvanceVerify(RCAdvanceVerifyDto request);
        Task<ApiResponse<JsonElement>> GetRCReverseVerify(RCReverseDto request);
    }
}
