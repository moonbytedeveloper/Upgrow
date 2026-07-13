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
    public interface IRequestSchemaService : IMasterService<RequestSchemaDto, RequestSchemaCommand>
    {
        Task<List<RequestSchemaDto>> GetAllActiveAsync();
        Task<RequestSchemaDto> SaveAndReturnAsync(RequestSchemaCommand command, string userUuid, string ip);

        Task<RequestSchemaDto?> GetByVersionUuidAsync(string versionUuid);

        Task<List<ReqResSchemaFieldDto>> GetFieldsBySchemaUuidAsync(string schemaUuid);

        Task<(RequestSchemaDto? Schema, List<ReqResSchemaFieldDto> Fields)> GetCompleteSchemaAsync(string versionUuid);

    }
}
