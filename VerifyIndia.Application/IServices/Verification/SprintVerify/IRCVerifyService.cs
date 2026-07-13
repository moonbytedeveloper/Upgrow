using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Verification.SprintVerify;
using VerifyIndia.Application.Helper;

namespace VerifyIndia.Application.IServices.Verification.SprintVerify
{
    public interface IRCVerifyService
    {
        public Task<ApiResponse<JsonElement>> GetRCVerify(RCVerifyDto request);
        public Task<ApiResponse<JsonElement>> GetRCAdvanceVerify(RCAdvanceVerifyDto request);
        Task<ApiResponse<JsonElement>> GetRCReverseVerify(RCReverseDto request);
    }
}
