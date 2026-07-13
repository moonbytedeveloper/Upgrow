using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;

namespace VerifyIndia.Application.IServices
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
