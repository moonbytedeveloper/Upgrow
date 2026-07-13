using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Master
{
    /// <summary>
    /// Service contract for Master API operations.
    /// </summary>
    public interface IMasterApiService : IMasterService<MasterApiDto, MasterApiCommand>
    {
        /// <summary>
        /// Returns all active APIs, used by UI to determine next DisplayOrder.
        /// </summary>
        Task<List<MasterApiDto>> GetAllActiveAsync();
      Task<MasterApiDto> SaveAsync(
    MasterApiCommand command,
    string userUUID,
    string ipAddress,
    bool saveChanges);

    }
}
