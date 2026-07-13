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
    public interface IApiXLanguageContentService : IMasterService<ApiXLanguageContentDto, ApiXLanguageContentCommand>

    {
        Task<List<ApiXLanguageContentDto>> GetAllActiveAsync();
        Task<List<ApiXLanguageContentDto>> GetAllAsync();
        Task<ApiXLanguageContentDto> SaveAndReturnAsync(ApiXLanguageContentCommand command, string userUuid, string ip);

        Task<ApiXLanguageContentDto?> GetByVersionAndLanguageAsync(string versionUUID, string languageUUID);
        Task<List<ApiXLanguageContentDto>> GetByVersionAsync(string versionUUID);
        Task<bool> CheckDuplicateAsync(ApiXLanguageContentCommand command);
    }
}
   
