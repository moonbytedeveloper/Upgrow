using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.DTO.Api;

namespace VerifyIndia.Application.IServices.Api
{
    public interface IReqResSchemaFieldService
    {
 
        Task<List<ReqResSchemaFieldDto>> GetBySchemaUuidAsync(string schemaUuid);
    }
}