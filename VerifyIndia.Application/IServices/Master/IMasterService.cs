using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Domain.Common;

namespace VerifyIndia.Application.IServices.Master
{
    public interface IMasterService<TDto, TCommand>
        where TDto : class
        where TCommand : class
    {
        Task<TDto?> GetByUuidAsync(string uuid);

        Task<PagedResult<TDto>> GetPagedAsync(DataTableRequest request);

        //Task SaveAsync(TCommand command, string userUuid, string ip);
        Task SaveAsync(TCommand command, string userUuid, string ip, bool saveChanges = true);
        Task<List<MasterDropDownDto>> GetDropdownAsync(Func<TCommand, string> displaySelector);
        Task DeleteAsync(string uuid, string userUuid, string ip);

        /// <summary>
        /// Toggles the IsActive status of an entity
        /// </summary>
        Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip);

        Task SaveChangesAsync();

    }
}
