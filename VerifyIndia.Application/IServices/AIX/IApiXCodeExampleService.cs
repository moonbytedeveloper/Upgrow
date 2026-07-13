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
    public interface IApiXCodeExampleService : IMasterService<ApiXCodeExampleDto, ApiXCodeExampleCommand>
    {
        Task<List<ApiXCodeExampleDto>> GetAllActiveAsync();

        Task<ResponseExampleDto?> GetByUuidAsync(string uuid);

        Task<List<ResponseExampleDto>> GetByStatusCodeUuidAsync(string statusCodeUuid);
       

    }
}
