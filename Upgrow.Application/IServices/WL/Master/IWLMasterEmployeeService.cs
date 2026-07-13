using System;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.WL.Master;
using Upgrow.Application.IServices.Master;

namespace Upgrow.Application.IServices.WL.Master
{
    public interface IWLMasterEmployeeService : IMasterService<WLMasterEmployeeDto, WLMasterEmployeeCommand>
    {
        Task ChangePasswordAsync(string uuid, string currentPassword, string newPassword);

    }
}
