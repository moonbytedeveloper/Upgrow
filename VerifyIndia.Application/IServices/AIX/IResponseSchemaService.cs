using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices.AIX
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
