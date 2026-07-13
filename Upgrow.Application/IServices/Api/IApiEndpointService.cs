using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.Api
{
    public interface IApiEndpointService : IMasterService<ApiEndpointDto, ApiEndpointCommand>
    {
        Task<List<ApiEndpointDto>> GetByApiAsync(string apiUuid);
        Task<ApiEndpointDto> GetByApiUuidAsync(string apiUuid);
    }
}

