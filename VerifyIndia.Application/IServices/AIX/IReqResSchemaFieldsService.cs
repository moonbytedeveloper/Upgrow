using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.AIX
{
    public interface IReqResSchemaFieldsService : IMasterService<ReqResSchemaFieldDto, ReqResSchemaFieldCommand>
    {
        Task<ReqResSchemaFieldDto> SaveAndReturnAsync(ReqResSchemaFieldCommand command, string userUuid, string ip);
        Task<List<ReqResSchemaFieldDto>> GetAllActiveAsync();
        Task<List<ReqResSchemaFieldDto>> GetAllAsync();
        Task<bool> CheckDuplicateAsync(ReqResSchemaFieldCommand command);

    }
}
    