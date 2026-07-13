using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.DTO.Api;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.AIX
{
    public interface IApiXVersionService : IMasterService<ApixVersionDto, ApixVersionCommand>
    {
        Task<int> CreateAsync(ApixVersionDto dto);
        Task<ApixVersionDto> SaveAndReturnAsync(ApixVersionCommand command, string userUuid, string ip);
        Task<bool> CanActivateAsync(string versionUuid);

        Task<List<ApiXVersionDto>> GetAllActiveAsync();
        Task<List<ApiXVersionDto>> GetByApiXCategoryUUIDAsync(string apiXCategoryUUID);


    }
}
