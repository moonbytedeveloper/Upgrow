using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.AIX;

namespace VerifyIndia.Application.IServices.Api
{
    public interface IApiXStatusCodeService
    {
        Task<ResponseStatusDto?> GetByUuidAsync(string uuid);
 
        Task<List<ResponseStatusDto>> GetAllActiveAsync();
 
        Task<List<ResponseStatusDto>> GetByUuidsAsync(List<string> uuids);
    }
}