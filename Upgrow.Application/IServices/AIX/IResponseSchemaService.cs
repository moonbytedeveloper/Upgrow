using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices.AIX
{
    public interface IResponseSchemaService : IMasterService<ResponseSchemaDto, ResponseSchemaCommand>
    {
        Task<List<ResponseSchemaDto>> GetAllActiveAsync();
        Task<ResponseSchemaDto> SaveAndReturnAsync(ResponseSchemaCommand command, string userUuid, string ip);
        Task<ResponseSchema?> GetByUuidAsync(string uuid);
        Task<List<ReqResSchemaFields>> GetFieldsBySchemaUuidAsync(string schemaUuid);
        Task<ResponseSchemaSectionDto?> GetResponseSchemaBySectionAsync(string responseSchemaUuid);
    }
}
