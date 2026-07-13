using Upgrow.Application.DTO.AIX;
using Upgrow.Application.DTO.Api;

namespace Upgrow.Application.IServices.Api
{
    public interface IReqResSchemaFieldService
    {
 
        Task<List<ReqResSchemaFieldDto>> GetBySchemaUuidAsync(string schemaUuid);
    }
}