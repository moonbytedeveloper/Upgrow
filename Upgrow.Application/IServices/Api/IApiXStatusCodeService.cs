using System.Collections.Generic;
using System.Threading.Tasks;
using Upgrow.Application.DTO.AIX;

namespace Upgrow.Application.IServices.Api
{
    public interface IApiXStatusCodeService
    {
        Task<ResponseStatusDto?> GetByUuidAsync(string uuid);
 
        Task<List<ResponseStatusDto>> GetAllActiveAsync();
 
        Task<List<ResponseStatusDto>> GetByUuidsAsync(List<string> uuids);
    }
}