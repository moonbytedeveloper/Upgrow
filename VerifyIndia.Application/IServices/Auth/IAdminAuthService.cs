using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Master;

namespace VerifyIndia.Application.IServices.Auth
{
    public interface IAdminAuthService
    {
        Task<ResultDto?> ValidateCredentialsAsync(string username, string password, string? ipAddress = null);
        Task<(bool IsSuccess, string Message, string? Token)> GeneratePasswordResetTokenAsync(string email);

        Task<(bool IsSuccess, string Message)> ResetPasswordAsync(string token, string newPassword);

        Task<MasterEmployeeDto?> GetUserByEmailAsync(string email);
    }
}
