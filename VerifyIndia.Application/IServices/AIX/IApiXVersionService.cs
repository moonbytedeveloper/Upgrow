using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.AIX
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
