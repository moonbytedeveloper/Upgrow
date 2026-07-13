using AuthenticateIndia.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Auth;
using VerifyIndia.Application.Common;
using VerifyIndia.Application.Constant;
using VerifyIndia.Application.DTO.Auth;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.Notification;
using VerifyIndia.Application.Interfaces.Notification;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Auth;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Services;
using VerifyIndia.Application.Services.Auth;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.Enums;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Infrastructure;
using VerifyIndia.Infrastructure.Filters;
using VerifyIndia.Infrastructure.Repositories;
using static VerifyIndia.Application.Constants;

namespace UpgrowAdminPanel.Controllers
{
    [ActivityLog]
    public class AuthController : BaseController
    {
        private readonly IAdminAuthLogsRepository _adminAuthLogsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminAuthService _adminAuthService;
        private readonly IMasterEmailTemplateService _emailTemplateService;
        private readonly INotificationChannel _notificationChannel;
        private readonly AppInfo _appInfo;
        private readonly ILoginAttemptService _loginAttemptService;
        private readonly ILoginLogoutService _loginLogoutService;
        private readonly IDataTableParser _dataTableParser;
        private readonly IMasterEmployeeRepository _masterEmployeeRepository;
        private readonly IDomainResolverService _tenantDomainResolver;
        private readonly IAdminAuthLogsService _adminAuthLogsService;
        private readonly IPasswordPolicyService _policyService;
        private readonly INotificationOrchestrator _notificationOrchestrator;
        private readonly IAppSettingService _appSettingService;
        public AuthController(
            IAppSettingService appSettingService,
            IEncryptionService encryptionService,
            INotificationOrchestrator notificationOrchestrator,
            IAdminAuthService adminAuthService,
            INotificationChannel notificationChannel,
            IMasterEmailTemplateService emailTemplateService,
            IDataTableParser dataTableParser,
            IDomainResolverService tenantDomainResolver,
            IAdminAuthLogsRepository adminAuthLogsRepository,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppInfo> appOptions,
            ILoginAttemptService loginAttemptService,
            IMasterEmployeeRepository masterEmployeeRepository,
            IAdminAuthLogsService adminAuthLogsService,
            ILoginLogoutService loginLogoutService,
            IPasswordPolicyService policyService
            ) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _adminAuthService = adminAuthService;
            _notificationChannel = notificationChannel;
            _emailTemplateService = emailTemplateService;
            _appInfo = appOptions.Value;
            _adminAuthLogsRepository = adminAuthLogsRepository;
            _httpContextAccessor = httpContextAccessor;
            _tenantDomainResolver = tenantDomainResolver;
            _loginAttemptService = loginAttemptService;
            _dataTableParser = dataTableParser;
            _loginLogoutService = loginLogoutService;
            _masterEmployeeRepository = masterEmployeeRepository;
            _adminAuthLogsService = adminAuthLogsService;
            _policyService = policyService;
            _notificationOrchestrator = notificationOrchestrator; 
            _appSettingService = appSettingService;
        }

        [HttpGet]
        public IActionResult Login()
        {
#if DEBUG
            if (User?.Identity?.IsAuthenticated == true &&
                User.HasClaim(c => c.Type == ClaimTypes.Name) &&
                User.HasClaim(c => c.Type == ClaimConstants.UUID) &&
                User.HasClaim(c => c.Type == ClaimConstants.RoleUUID) &&
                User.HasClaim(c => c.Type == ClaimConstants.IP) &&
                User.HasClaim(c => c.Type == ClaimConstants.EmployeeCode))
            {
                return RedirectToAction("Index", "Dashboard");
            }
#endif
            return View();
        }



        /*[HttpGet]
        public async Task<IActionResult> GetLoginAttemptLogs(string? userUuid, string? search)
        {
            try
            {
                var query = _context.LoginAttempts.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(userUuid))
                {
                    userUuid = userUuid.Trim();
                    query = query.Where(x => x.UserUUID == userUuid);
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim().ToLower();

                    query = query.Where(x =>
                        (x.UserName != null && x.UserName.ToLower().Contains(search)) ||
                        (x.FailureReason != null && x.FailureReason.ToLower().Contains(search)));
                }

                var items = await query
                    .OrderByDescending(x => x.AttemptTime)
                    .Take(200)
                    .Select(x => new
                    {
                        date = ToUserTimeString(x.AttemptTime),
                        user = x.UserName ?? "",
                        login = x.IsSuccess,
                        reason = x.FailureReason ?? "",
                        ip = x.IpAddress ?? ""
                    })
                    .ToListAsync();

                var items = await query
                  .OrderByDescending(x => x.AttemptTime)
                  .Take(200)
                  .ToListAsync();

                var result = items.Select(x => new
                {
                    date = ToUserTimeString(x.AttemptTime),
                    user = x.UserName ?? "",
                    login = x.IsSuccess,
                    reason = x.FailureReason ?? "",
                    ip = x.IpAddress ?? ""
                }).ToList();

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = Array.Empty<object>() });
            }
        }*/



        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> GetLoginAttemptLogs(string? userUuid)
        => GetPagedDataAsync<LoginAttemptDto, LoginAttemptCommand>(_loginAttemptService, dto => new Dictionary<string, object>
        {
            ["date"] = ToUserTimeString(dto.AttemptTime),
            ["user"] = dto.UserName ?? "",
            ["status"] = dto.IsSuccess ? "Success" : "Failed",
            ["reason"] = dto.FailureReason ?? "",
            ["ip"] = dto.IpAddress ?? ""
        });
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetLoginAttemptLogs(string? userUuid)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var request = _dataTableParser.ParseRequest();
                var result = await _loginAttemptService.GetPagedByUserAsync(request, userUuid);

                var srNo = request.Start + 1;

                return Json(new
                {
                    draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                    data = result.Items.Select(x =>
                    {
                        var row = new Dictionary<string, object>
                        {
                            ["date"] = ToUserTimeString(x.AttemptTime),
                            ["user"] = x.UserName ?? "",
                            ["status"] = x.IsSuccess ? "Success" : "Failed",
                            ["reason"] = x.FailureReason ?? "",
                            ["ip"] = x.IpAddress ?? ""
                        };
                        row["srno"] = srNo++;
                        return row;
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>(),
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetLoginLogoutLogs(string? userUuid)
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var request = _dataTableParser.ParseRequest();
                var result = await _loginLogoutService.GetPagedByUserAsync(request, userUuid);

                /*var srNo = request.Start + 1;*/


                string userName = "N/A";
                if (!string.IsNullOrWhiteSpace(userUuid))
                {
                    try
                    {
                        var employee = await _masterEmployeeRepository.GetByUuidAsync(userUuid);
                        if (employee != null)
                        {
                            userName = $"{employee.FirstName} {employee.LastName}";
                        }
                    }
                    catch
                    {
                        userName = userUuid;
                    }
                }


                var data = result.Items.Select(x =>
                {
                    var row = new Dictionary<string, object>
                    {
                        ["date"] = ToUserTimeString(x.CreatedAt),
                        ["user"] = userName,
                        ["activity"] = x.Activity ?? "N/A",
                        ["ip"] = x.IpAddress ?? ""
                    };
                    /* row["srno"] = srNo++;*/
                    return row;
                }).ToList();

                return Json(new
                {
                    draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                    data
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>(),
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminPanelLoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }


            var result = await _adminAuthService.ValidateCredentialsAsync(
                login.Username,
                login.Password,
                Utils.GetLocalIPAddress());

            if (result is null)
            {
                SetErrorMessage("Enter valid credentials.");
                return View(login);
            }

            await SignInUserAsync(result);


            return RedirectToAction("Index", "Dashboard");
        }

        private async Task SignInUserAsync(ResultDto result)
        {
            // Sign out any existing authentication to invalidate previous cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.FullName ?? string.Empty),
        new Claim(ClaimConstants.UUID, result.UUID ?? string.Empty),
        new Claim(ClaimConstants.RoleUUID, result.RoleUUID ?? string.Empty),
        new Claim(ClaimConstants.IP, result.IP ?? string.Empty),
        new Claim(ClaimConstants.ProfilePic, result.ProfilePic ?? string.Empty),
        new Claim(ClaimConstants.EmployeeCode, result.EmployeeCode ?? string.Empty),
        new Claim(ClaimConstants.Honorific, result.Honorific ?? string.Empty)




            };
            if (result.LastLogin.HasValue)
            {

                var lastLogin = result.LastLogin.Value;
                DateTime lastLoginUtc;
                if (lastLogin.Kind == DateTimeKind.Unspecified)
                {
                    lastLoginUtc = DateTime.SpecifyKind(lastLogin, DateTimeKind.Local).ToUniversalTime();
                }
                else if (lastLogin.Kind == DateTimeKind.Local)
                {
                    lastLoginUtc = lastLogin.ToUniversalTime();
                }
                else
                {
                    lastLoginUtc = lastLogin;
                }

                claims.Add(new Claim("LastLogin", lastLoginUtc.ToString("o"))); // always store claim as UTC ISO
            }
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userUuid = User?.FindFirst(ClaimConstants.UUID)?.Value;
                var context = _httpContextAccessor.HttpContext;
                if (!string.IsNullOrWhiteSpace(userUuid))
                {
                    await _adminAuthLogsService.AddAsync(new AdminAuthLogDto
                    {

                        UserUUID = userUuid,
                        Activity = "Logout",
                        CreatedAt = Utils.GetCurrentUtcTime(),
                        IPAddress = context != null ? Utils.GetUserIp(context) : "Unknown",
                        UserAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString()
                    });
                }
            }
            catch
            {
                // Do not block logout if logging fails
            }

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Set aggressive cache control headers for this response
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate, max-age=0, private";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";
            Response.Headers["Surrogate-Control"] = "no-store";

            return RedirectToAction(nameof(Login));
        }


        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Forgot Password

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                SetErrorMessage("Email id is required.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                SetErrorMessage("Enter the valid email address.");
                return View(model);
            }

            var user = await _adminAuthService.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                SetErrorMessage("User not found.");
                return View(model);
            }

            var result = await _adminAuthService.GeneratePasswordResetTokenAsync(model.Email);

            if (!result.IsSuccess)
            {
                SetErrorMessage(result.Message);
                return View(model);
            }

            var token = result.Token;
            var resetLink = $"{GetBaseUrl()}/Auth/ResetPassword?token={token}";
            var expiryInSeconds = await _appSettingService.GetIntValueAsync(AppSettingKeys.FORGOT_PASSWORD_EXPIRY_SECONDS, 900);

            if (expiryInSeconds <= 0)
            {
                expiryInSeconds = 900; // Default to 15 minutes
            }
            var expiryInMinutes = expiryInSeconds / 60;
            var request = new NotificationRequestDto
            {
                UserId = user.UUID,
                EventCode = NotificationEvents.ResetPassword,
                Email = model.Email,
                Channels = new List<NotificationChannel>
                    {
                        NotificationChannel.Email
                    },
                Variables = new Dictionary<string, object>
                    {
                        { "user_name", $"{user.FirstName} {user.LastName}" },
                        { "ResetLink", resetLink },
                        { "expiry_time", expiryInMinutes },
                        { "company_name", _appInfo.Name }
                    }
            };

            try
            {
                await _notificationOrchestrator.SendAsync(request);
            }
            catch (Exception ex)
            {
                SetErrorMessage("Failed to send reset email. " + ex.Message);
                return View(model);
            }

            SetSuccessMessage("Password reset link has been sent to your email.");
            return RedirectToAction(nameof(ForgotPassword));
        }
        #endregion

        #region Reset Password

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                SetErrorMessage("Invalid password reset link.");
                return RedirectToAction("ForgotPassword");
            }
            var policy = await _policyService.GetFirstAsync();
            var model = new ResetPasswordDto
            {
                Token = token
            };
            model.PasswordPolicy = policy;
            model.PasswordPolicyDescription = BuildPasswordPolicyDescription(policy);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var policy = await _policyService.GetFirstAsync();
            model.PasswordPolicy = policy;
            model.PasswordPolicyDescription = BuildPasswordPolicyDescription(policy);
            if (string.IsNullOrWhiteSpace(model.Token) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                SetErrorMessage("Token and new password are required.");
                return View(model);
            }
            var validationMessage = ValidatePassword(model.NewPassword, model.PasswordPolicy);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                ModelState.AddModelError("NewPassword", validationMessage);
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }
            // Updated service now returns a tuple
            var result = await _adminAuthService.ResetPasswordAsync(model.Token, model.NewPassword);

            if (!result.IsSuccess)
            {
                SetErrorMessage(result.Message);
                return View(model);
            }

            SetSuccessMessage("Password updated successfully!");
            return RedirectToAction("Login");
        }

        private string ValidatePassword(string password, PasswordPolicyDto policy)
        {
            if (string.IsNullOrEmpty(password))
                return "Password is required.";

            if (password.Length < policy.MinLength)
                return $"Password must be at least {policy.MinLength} characters.";

            if (policy.UpperCase == true && !password.Any(char.IsUpper))
                return "Password must contain at least one uppercase letter.";

            if (policy.LowerCase == true && !password.Any(char.IsLower))
                return "Password must contain at least one lowercase letter.";

            if (policy.AllowDigit == true && !password.Any(char.IsDigit))
                return "Password must contain at least one digit.";

            if (policy.AllowSpecialChar == true &&
                !password.Any(c => policy.SpecialCharacters.Contains(c)))
                return $"Password must contain at least one special character ({policy.SpecialCharacters}).";

            return null; // valid
        }
        private string BuildPasswordPolicyDescription(PasswordPolicyDto policy)
        {
            if (policy == null || policy.IsActive != true)
                return "Password policy is not configured.";

            var rules = new List<string>();

            rules.Add($"Minimum length: {policy.MinLength}");

            if (policy.UpperCase == true)
                rules.Add("Must include uppercase letters (A-Z)");

            if (policy.LowerCase == true)
                rules.Add("Must include lowercase letters (a-z)");

            if (policy.AllowDigit == true)
                rules.Add("Must include numbers (0-9)");

            if (policy.AllowSpecialChar == true)
                rules.Add($"Must include special characters ({policy.SpecialCharacters})");

            return string.Join(" | ", rules);
        }
        #endregion

    }
}