using System;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.IServices.WL.Master
{
    public interface IWLMasterEmployeeService : IMasterService<WLMasterEmployeeDto, WLMasterEmployeeCommand>
    {
        Task ChangePasswordAsync(string uuid, string currentPassword, string newPassword);

    }
}
