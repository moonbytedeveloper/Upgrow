using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.DTO.Api;
 
namespace VerifyIndia.Application.IServices.Master
{
    public interface IApiXHeaderService : IMasterService<ApiXHeaderDto, ApiXHeaderCommand>
    {
        Task<List<ApiXHeaderDto>> GetByApiXVersionUUIDAsync(string apiXVersionUUID);

    }
}
