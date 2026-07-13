using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.Api;
 
namespace Upgrow.Application.IServices.Master
{
    public interface IApiXHeaderService : IMasterService<ApiXHeaderDto, ApiXHeaderCommand>
    {
        Task<List<ApiXHeaderDto>> GetByApiXVersionUUIDAsync(string apiXVersionUUID);

    }
}
