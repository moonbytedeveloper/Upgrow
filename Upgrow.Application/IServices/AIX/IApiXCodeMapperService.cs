using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.AIX
{
    public interface IApiXCodeMapperService : IMasterService<ApiXCodeMapperDto, ApiXCodeMapperCommand>
    {
        Task<List<ApiXCodeMapperDto>> GetAllActiveAsync();
        Task<List<ApiXCodeMapperDto>> GetAllAsync();
        Task<ApiXCodeMapperDto> SaveAndReturnAsync(ApiXCodeMapperCommand command, string userUuid, string ip);

        Task<ApiXCodeMapperDto?> GetByUuidAsync(string uuid);

        Task<List<ApiXCodeMapperDto>> GetByVersionUuidAsync(string versionUuid);

        Task<List<ApiXCodeMapperDto>> GetByStatusUuidAsync(string statusUuid);

        Task<bool> CheckDuplicateAsync(ApiXCodeMapperCommand command);
    }
}
