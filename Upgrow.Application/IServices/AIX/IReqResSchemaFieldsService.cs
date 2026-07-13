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
    public interface IReqResSchemaFieldsService : IMasterService<ReqResSchemaFieldDto, ReqResSchemaFieldCommand>
    {
        Task<ReqResSchemaFieldDto> SaveAndReturnAsync(ReqResSchemaFieldCommand command, string userUuid, string ip);
        Task<List<ReqResSchemaFieldDto>> GetAllActiveAsync();
        Task<List<ReqResSchemaFieldDto>> GetAllAsync();
        Task<bool> CheckDuplicateAsync(ReqResSchemaFieldCommand command);

    }
}
    