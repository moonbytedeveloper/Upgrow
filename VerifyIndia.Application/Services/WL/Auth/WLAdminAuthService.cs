using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Entities.Auth;
using VerifyIndia.Domain.Entities.WL.Master;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.WL;

namespace VerifyIndia.Application.Services.WL.Auth
{
    public class WLAdminAuthService : IWLAdminAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWLMasterEmployeeRepository _employeeRepo;
        private readonly IWLMasterHonorificService _honorificService;
        private readonly IAdminAuthLogsService _adminAuthLogsService;
        private readonly IAdminAuthLogsRepository _adminAuthLogsRepository;
        private readonly IPasswordResetRepository _tokenRepo;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly ILoginAttemptLogsService _loginAttemptLogsService;
        private readonly IPasswordHasher<WL_MasterEmployee> _passwordHasher = new PasswordHasher<WL_MasterEmployee>();

        public WLAdminAuthService(IWLMasterEmployeeRepository employeeRepo, IWLMasterHonorificService honorificService, IHttpContextAccessor httpContextAccessor, IPasswordHasher<WL_MasterEmployee> passwordHasher, IAdminAuthLogsService adminAuthLogsService, IAdminAuthLogsRepository adminAuthLogsRepository, IPasswordResetRepository tokenRepo, ILoginHistoryRepository loginHistoryRepository, ILoginAttemptLogsService loginAttemptLogsService)
        {
            _employeeRepo = employeeRepo;
            _honorificService = honorificService;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
            _adminAuthLogsService = adminAuthLogsService;
            _adminAuthLogsRepository = adminAuthLogsRepository;
            _tokenRepo = tokenRepo;
            _loginHistoryRepository = loginHistoryRepository;
            _loginAttemptLogsService = loginAttemptLogsService;
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

            employee.Password = _passwordHasher.HashPassword(employee, newPassword);
            await _employeeRepo.UpdateAsync(employee);

            entry.IsUsed = true;
            await _tokenRepo.UpdateAsync(entry);

            return (true, "Password updated successfully.");
        }

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
                catch (Exception ex)
                {
                   
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
                   
                }

                return null;
            }

            if (verifyResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                employee.Password = _passwordHasher.HashPassword(employee, password);
                await _employeeRepo.UpdateAsync(employee); // implement if not exists
            }

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
                
            }


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
               
            }

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
                    
                    Console.WriteLine($"Login history error: {ex.Message}");
                }
            }
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
        async Task<WLMasterEmployeeDto?> IWLAdminAuthService.GetUserByEmailAsync(string email)
        {
            var employee = await _employeeRepo.GetByEmailAsync(email);
            if (employee == null) return null;

            return new WLMasterEmployeeDto
            {
                UUID = employee.UUID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                EmailId = employee.EmailId
            };
        }
    }
}