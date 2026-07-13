using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;

namespace Upgrow.Application.IServices
{
    public interface IPinnedService : IMasterService<PinnedServiceDto, PinnedServiceCommand>
    {
        Task<List<PinnedServiceDto>> GetAllActiveAsync();
        Task<PinnedServiceDto?> GetByUuidAsync(string uuid);
        /// <summary>
        /// Returns true if there exists an active pinned record for the API.
        /// If customerUuid is provided the check is scoped to that customer; otherwise checks globally.
        /// </summary>
        Task<bool> IsPinnedAsync(string apiUuid, string? customerUuid = null);


    }
}
