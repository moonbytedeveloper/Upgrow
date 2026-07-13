using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.AIX
{
    public interface IApiXCodeExampleService : IMasterService<ApiXCodeExampleDto, ApiXCodeExampleCommand>
    {
        Task<List<ApiXCodeExampleDto>> GetAllActiveAsync();

        Task<ResponseExampleDto?> GetByUuidAsync(string uuid);

        Task<List<ResponseExampleDto>> GetByStatusCodeUuidAsync(string statusCodeUuid);
       

    }
}
