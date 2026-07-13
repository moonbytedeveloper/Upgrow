using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL.Master;

namespace Upgrow.Application.IServices.Auth
{
    public interface IWLAdminAuthService
    {
        Task<ResultDto?> ValidateCredentialsAsync(string username, string password, string? ipAddress = null);
        Task<(bool IsSuccess, string Message, string? Token)> GeneratePasswordResetTokenAsync(string email);

        Task<(bool IsSuccess, string Message)> ResetPasswordAsync(string token, string newPassword);

        Task<WLMasterEmployeeDto?> GetUserByEmailAsync(string email);
    }
}
