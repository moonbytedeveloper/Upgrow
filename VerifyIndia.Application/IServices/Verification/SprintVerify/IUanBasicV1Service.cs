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
    public interface IUanBasicV1Service
    {
        Task<ApiResponse<JsonElement>> GetUanBasicV1(UanBasicV1Dto request);
    }
}
