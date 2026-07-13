using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.DTO.Auth;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Auth;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.Master;

namespace Upgrow.Application.Services.Auth
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly ILoginAttemptLogsService _loginAttemptLogsService;
        private readonly ILoginAttemptRepository _loginAttemptRepository;
        private readonly IAdminAuthLogsService _adminAuthLogsService;
        private readonly IAdminAuthLogsRepository _adminAuthLogsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterEmployeeRepository _employeeRepo;
        private readonly IPasswordResetRepository _tokenRepo;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly IMasterHonorificService _honorificService;
        private readonly PasswordHasher<Master_Employee> _passwordHasher = new PasswordHasher<Master_Employee>();

        public AdminAuthService(IMasterEmployeeRepository employeeRepo, IPasswordResetRepository tokenRepo, ILoginHistoryRepository loginHistoryRepository, IMasterHonorificService honorificService, ILoginAttemptRepository loginAttemptRepository,
        IAdminAuthLogsRepository adminAuthLogsRepository, IHttpContextAccessor httpContextAccessor, IAdminAuthLogsService adminAuthLogsService, ILoginAttemptLogsService loginAttemptLogsService)
        {
            _employeeRepo = employeeRepo;
            _tokenRepo = tokenRepo;
            _loginHistoryRepository = loginHistoryRepository;
            _honorificService = honorificService;
            _loginAttemptRepository = loginAttemptRepository;
            _adminAuthLogsRepository = adminAuthLogsRepository;
            _adminAuthLogsService = adminAuthLogsService;
            _httpContextAccessor = httpContextAccessor;
            _loginAttemptLogsService = loginAttemptLogsService;
        }

        //public async Task<ResultDto?> ValidateCredentialsAsync(string username, string password, string? ipAddress = null)
        //{
        //    var employee = await _employeeRepo.GetByCredentialsAsync(username);

        //    if (employee is null)
        //        return null;
        //    var loginPassword = _passwordHasher.VerifyHashedPassword( employee, employee.Password, password);

        //    if (loginPassword == PasswordVerificationResult.Failed)
        //        return null;
        //    DateTime? previousLogin = null;
        //    try
        //    {
        //        previousLogin = await _loginHistoryRepository.GetLastLoginAsync(employee.UUID ?? string.Empty);
        //    }
        //    catch
        //    {
        //        // swallow/log as appropriate — we still want to sign in user even if history fails
        //    }
        //    string? honorificTitle = null;
        //    if (!string.IsNullOrWhiteSpace(employee.HonorificUUID))
        //    {
        //        try
        //        {
        //            var honDto = await _honorificService.GetByUuidAsync(employee.HonorificUUID);
        //            // DTO expected to contain Title (adjust if different)
        //            honorificTitle = honDto?.Title;
        //        }
        //        catch
        //        {
        //            // swallow/log
        //        }
        //    }

        //    var result = new ResultDto
        //    {
        //        UUID = employee.UUID,
        //        FullName = employee.FirstName + " " + employee.LastName,
        //        RoleUUID = employee.RoleUUID, // or whatever maps to role
        //        IP = ipAddress,
        //        LastLogin = previousLogin,
        //        ProfilePic = string.IsNullOrEmpty(employee.Profile_URL) ? null : employee.Profile_URL,
        //        EmployeeCode = employee.EmployeeCode,
        //        Honorific = honorificTitle
        //    };
        //    try
        //    {
        //        var currentIp = string.IsNullOrWhiteSpace(ipAddress) ? Utils.GetLocalIPAddress() : ipAddress;
        //        // Save server local time instead of UTC so DB shows local time directly
        //        await _loginHistoryRepository.RecordLoginAsync(employee.UUID ?? string.Empty, currentIp, Utils.GetCurrentTime());
        //    }
        //    catch
        //    {
        //        // swallow/log — do not block sign-in on audit failure
        //    }
        //    return result;
        //}
        public async Task<ResultDto?> ValidateCredentialsAsync(
        string username,
        string password,
        string? ipAddress = null)
        {
            // 1. Get user by username
            var employee = await _employeeRepo.GetByCredentialsAsync(username);

            if (employee == null)
            {
                try
                {
                    var context = _httpContextAccessor.HttpContext;
                    
                    await _loginAttemptLogsService.AddAsync(new LoginAttemptDto
                    {
                        UserUUID = null,
                        UserName = username,
                        AttemptTime = Utils.GetCurrentUtcTime(),
                        IsSuccess = false,
                        FailureReason = "User Not Found",
                        IpAddress = context != null ? Utils.GetUserIp(context) : "Unknown" 
                    });
                }
                catch(Exception ex)
                {
                    // swallow/log as appropriate — we still want to sign in user even if history fails
                }
                return null;
            }
                

            // 2. Verify password
            var verifyResult = _passwordHasher.VerifyHashedPassword(
                employee,
                employee.Password,
                password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                try
                {
                    var context = _httpContextAccessor.HttpContext;
                    await _loginAttemptLogsService.AddAsync(new LoginAttemptDto
                    {
         
                        UserUUID = employee.UUID,
                        UserName = username,
                        AttemptTime = Utils.GetCurrentUtcTime(),
                        IsSuccess = false,
                        FailureReason = "Invalid Password",
                        IpAddress = context != null ? Utils.GetUserIp(context) : "Unknown"
 
                    });

                }
                catch (Exception ex)
                {
                    // swallow/log as appropriate — we still want to sign in user even if history fails
                }

                return null;
            }
                

            // 3. Optional: Rehash password if needed
            if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                employee.Password = _passwordHasher.HashPassword(employee, password);
                await _employeeRepo.UpdateAsync(employee); // implement if not exists
            }


            // 4) Record SUCCESS attempt
            try
            {
                var context = _httpContextAccessor.HttpContext;
                await _loginAttemptLogsService.AddAsync(new LoginAttemptDto
                {
 
                    UserUUID = employee.UUID,
                    UserName = username,
                    AttemptTime = Utils.GetCurrentUtcTime(),
                    IsSuccess = true,
                    FailureReason = null,
                    IpAddress = context != null ? Utils.GetUserIp(context) : "Unknown"
 
                });
            }
            catch (Exception ex)
            {
                // swallow/log as appropriate — we still want to sign in user even if history fails
            }


            // 5) Record auth activity log (LOGIN)
            try
            {
                var context = _httpContextAccessor.HttpContext;
                await _adminAuthLogsService.AddAsync(new AdminAuthLogDto
                {
               
                    UserUUID = employee.UUID,
                    Activity = "Login",
                    CreatedAt = Utils.GetCurrentUtcTime(),
                    IPAddress = context != null ? Utils.GetUserIp(context) : "Unknown",
                    UserAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString()
                });

            }
            catch (Exception ex)
            {
                // swallow/log as appropriate — we still want to sign in user even if history fails
            }

            // 4. Get last login safely
            DateTime? previousLogin = null;

            if (!string.IsNullOrEmpty(employee.UUID))
            {
                try
                {
                    previousLogin = await _loginHistoryRepository
                        .GetLastLoginAsync(employee.UUID ?? string.Empty);
                }
                catch (Exception ex)
                {
                    // ✅ Log instead of silent fail
                    Console.WriteLine($"Login history error: {ex.Message}");
                }
            }

            // 5. Get honorific title safely
            string? honorificTitle = null;

            if (!string.IsNullOrWhiteSpace(employee.HonorificUUID))
            {
                try
                {
                    var honDto = await _honorificService
                        .GetByUuidAsync(employee.HonorificUUID);

                    honorificTitle = honDto?.Title;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Honorific fetch error: {ex.Message}");
                }
            }

            // 6. Prepare result
            var result = new ResultDto
            {
                UUID = employee.UUID,
                FullName = $"{employee.FirstName} {employee.LastName}",
                RoleUUID = employee.RoleUUID,
                IP = ipAddress,
                LastLogin = previousLogin,
                ProfilePic = string.IsNullOrEmpty(employee.Profile_URL)
                    ? null
                    : employee.Profile_URL,
                EmployeeCode = employee.EmployeeCode,
                Honorific = honorificTitle
            };

            // 7. Record login history safely
            if (!string.IsNullOrEmpty(employee.UUID))
            {
                try
                {
                    var currentIp = string.IsNullOrWhiteSpace(ipAddress)
                        ? Utils.GetLocalIPAddress()
                        : ipAddress;

                    await _loginHistoryRepository.RecordLoginAsync(
                        employee.UUID,
                        currentIp,
                        Utils.GetCurrentUtcTime());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Login record error: {ex.Message}");
                }
            }

            return result;
        }
        public async Task<(bool IsSuccess, string Message, string? Token)> GeneratePasswordResetTokenAsync(string email)

        {
            var employee = await _employeeRepo.GetByEmailAsync(email);
            if (employee == null)
                return (false, "Email not registered.", null);

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddMinutes(15);

            var entry = new Auth_PassResetToken
            {
                EmployeeUUID = employee.UUID,
                Token = token,
                Expiry = expiry,
                IsUsed = false
            };

            await _tokenRepo.AddAsync(entry);

            return (true, "Password reset token created.", token);
        }

        public async Task<(bool IsSuccess, string Message)> ResetPasswordAsync(string token, string newPassword)
        {
            var entry = await _tokenRepo.GetByTokenAsync(token);
            if (entry == null || entry.IsUsed == true || (entry.Expiry.HasValue && entry.Expiry < DateTime.UtcNow))
                return (false, "Invalid or expired token.");

            var employee = await _employeeRepo.GetByUuidAsync(entry.EmployeeUUID);
            if (employee == null)
                return (false, "User not found.");

            // Hash new password
            employee.Password = _passwordHasher.HashPassword(employee, newPassword);

            // Save updated user
            await _employeeRepo.UpdateAsync(employee);

            entry.IsUsed = true;
            await _tokenRepo.UpdateAsync(entry);

            return (true, "Password updated successfully.");
        }

        public async Task<MasterEmployeeDto?> GetUserByEmailAsync(string email)
        {
            var employee = await _employeeRepo.GetByEmailAsync(email);
            if (employee == null) return null;

            return new MasterEmployeeDto
            {
                UUID = employee.UUID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailId = employee.EmailId
            };
        }

    }
}


