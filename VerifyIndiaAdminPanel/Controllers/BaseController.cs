using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moonbyte.UI;
using System.Security.Claims;
using TimeZoneConverter;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.Services;
using VerifyIndia.Application.Services.WL;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Infrastructure;
using static VerifyIndia.Application.Constants;

namespace VerifyIndiaAdminPanel.Controllers
{
    public class BaseController : Controller
    {

        private readonly IDataTableParser _dataTableParser;
        private readonly IDomainResolverService _tenantDomainResolver;
        private readonly IEncryptionService _encryptionService;

        private const string UserTimeZoneCookieKey = "user_tz";

        private static readonly Dictionary<string, string> TimeZoneFallbackMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Asia/Kolkata"] = "India Standard Time" 
        };

        public BaseController(IDataTableParser dataTableParser,
            IDomainResolverService tenantDomainResolver,
            IEncryptionService encryptionService)
        {
            _dataTableParser = dataTableParser;
            _tenantDomainResolver = tenantDomainResolver;
            _encryptionService = encryptionService;
        }

        #region Helper Methods

        protected async Task<IActionResult> GetPagedDataAsync<TDto>(
    Func<DataTableRequest, Task<PagedResult<TDto>>> getPaged,
    Func<TDto, IDictionary<string, object>> mapToDataTable)
    where TDto : class
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var request = _dataTableParser.ParseRequest();
                var result = await getPaged(request);

                var srNo = request.Start + 1;

                return Json(new
                {
                    draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                    data = result.Items.Select(x =>
                    {
                        var row = mapToDataTable(x);
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

        /// <summary>
        /// Generic method for DataTable paged data
        /// </summary>
        protected async Task<IActionResult> GetPagedDataAsync<TDto, TCommand>(
            IMasterService<TDto, TCommand> service,
            Func<TDto, IDictionary<string, object>> mapToDataTable,
             Func<TDto, bool>? filter = null)
            where TDto : class
            where TCommand : class
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var request = _dataTableParser.ParseRequest();
                var result = await service.GetPagedAsync(request);

                var srNo = request.Start + 1;
                var items = filter != null? result.Items.Where(filter): result.Items;
                return Json(new
                {
                    draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                    data = items.Select(x =>
                    {
                        var row = mapToDataTable(x);
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

        /// <summary>
        /// Generic method for Edit/Add view (existing behavior, no breaking changes)
        /// </summary>
        protected Task<IActionResult> EditMasterAsync<TDto, TCommand>(
            string? uuid,
            IMasterService<TDto, TCommand> service,
            Func<TCommand> createNew,
            Func<TDto, TCommand> mapToCommand,
            string viewName,
            string redirectAction)
            where TDto : class
            where TCommand : class
        {
            return EditMasterAsync<TDto, TCommand, TCommand>(
                uuid,
                service,
                createNew,
                mapToCommand,
                command => Task.FromResult(command),
                viewName,
                redirectAction);
        }

        /// <summary>
        /// Generic method for Edit/Add view with typed ViewModel projection
        /// </summary>
        protected async Task<IActionResult> EditMasterAsync<TDto, TCommand, TViewModel>(
            string? uuid,
            IMasterService<TDto, TCommand> service,
            Func<TCommand> createNew,
            Func<TDto, TCommand> mapToCommand,
            Func<TCommand, Task<TViewModel>> mapToViewModelAsync,
            string viewName,
            string redirectAction)
            where TDto : class
            where TCommand : class
            where TViewModel : class
        {
            TCommand command;

            if (string.IsNullOrEmpty(uuid))
            {
                command = createNew();
            }
            else
            {
                var dto = await service.GetByUuidAsync(uuid);

                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(redirectAction);
                }

                command = mapToCommand(dto);
            }

            var model = await mapToViewModelAsync(command);
            return View(viewName, model);
        }

        /// <summary>
        /// Generic method for Save (Create/Update)
        /// </summary>
        protected async Task<IActionResult> SaveMasterAsync<TDto, TCommand>(
    TCommand command,
    IMasterService<TDto, TCommand> service,
    string entityName,
    string viewName,
    string redirectAction)
    where TDto : class
    where TCommand : class, IMasterCommand
        {
            if (!ModelState.IsValid)
            {
                return View(viewName, command);
            }

            try
            {
                if (command is IEncryptable encryptable &&!string.IsNullOrWhiteSpace(encryptable.SensitiveValue) && encryptable.SensitiveValue != SecretMaskConstants.MaskedSecretValue)
                {
                    encryptable.SensitiveValue =
                        _encryptionService.Encrypt(encryptable.SensitiveValue);
                }
                //if (command is IEncryptable encryptable)
                //{
                //    // Skip fake password
                //    if (!string.IsNullOrWhiteSpace(encryptable.SensitiveValue) &&
                //        encryptable.SensitiveValue != "********")
                //    {
                //        encryptable.SensitiveValue =
                //            _encryptionService.Encrypt(encryptable.SensitiveValue);
                //    }
                //}

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
            ? $"{entityName} added successfully!"
            : $"{entityName} updated successfully!");

                await service.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

        
                return RedirectToAction(redirectAction);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View(viewName, command);
            }
        }

        /// <summary>
        /// Generic method for Save (Create/Update) when using a ViewModel that wraps a command.
        /// Keeps command-only SaveMasterAsync unchanged for existing screens.
        /// </summary>
        protected async Task<IActionResult> SaveMasterAsync<TDto, TCommand, TVm>(
            TVm vm,
            Func<TVm, TCommand?> getCommand,
            Func<TVm, Task<TVm>> repopulateVmAsync,
            IMasterService<TDto, TCommand> service,
            string entityName,
            string viewName,
            string redirectAction,
            string commandModelPrefix)
            where TDto : class
            where TCommand : class, IMasterCommand
            where TVm : class
        {
            if (vm == null)
                return BadRequest();

            var command = getCommand(vm);
            if (command == null)
                return BadRequest();

            // Validate nested command inside VM
            if (!TryValidateModel(command, commandModelPrefix))
            {
                vm = await repopulateVmAsync(vm);
                return View(viewName, vm);
            }

            try
            {
                await service.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? $"{entityName} added successfully!"
                    : $"{entityName} updated successfully!");

                return RedirectToAction(redirectAction);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm = await repopulateVmAsync(vm);
                return View(viewName, vm);
            }
        }

        /// <summary>
        /// Generic method for toggling IsActive status via AJAX
        /// </summary>
        protected async Task<IActionResult> ToggleActiveAsync<TDto, TCommand>(
            string uuid,
            IMasterService<TDto, TCommand> service,
            string entityName)
            where TDto : class
            where TCommand : class
        {
            try
            {
                var newStatus = await service.ToggleActiveAsync(uuid, GetUserUUID(), Utils.GetLocalIPAddress());
                return Json(new
                {
                    success = true,
                    isActive = newStatus,
                    message = $"{entityName} status updated successfully!"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        /// <summary>
        /// Helper to generate toggle switch HTML for DataTable action columns
        /// </summary>
        protected string GetToggleHtml(string uuid, bool isActive, string toggleAction, string entityName = "item")
        {
            // Use current controller name by default instead of hard-coded "Master".
            var controllerName = RouteData.Values["controller"]?.ToString() ?? "Master";
            var toggleUrl = Url.Action(toggleAction, controllerName)!;
            return ToggleTagHelper.GenerateHtml(uuid, isActive, toggleUrl, entityName);
        }

       

        #endregion        

        protected async Task<string?> ResolveCompanyNameFromRequestAsync()
        {
            try
            {
                var request = HttpContext?.Request;
                if (request == null) return null;

                var host = request.Host.Host?.Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(host)) return null;

                if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                    host = host.Substring(4);

                // Use the injected resolver (no direct DbContext/entity access here)
                if (_tenantDomainResolver != null)
                {
                    var identifier = await _tenantDomainResolver.ResolveCompanyIdentifierAsync(host);
                    return string.IsNullOrWhiteSpace(identifier) ? null : identifier.Trim();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Generic helper to load dropdown list for any service
        /// </summary>
        protected async Task<List<SelectListItem>> LoadDropdownAsync<TDto, TCommand>(
            IMasterService<TDto, TCommand> service,
            Func<TCommand, string> displaySelector)
            where TDto : class
            where TCommand : class
        {
            try
            {
                var items = await service.GetDropdownAsync(displaySelector);
                return items.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<SelectListItem>();
            }
        }

        #region Timezone info
        protected TimeZoneInfo GetUserTimeZoneInfo()
        {
            // 1) Prefer claim if you add it in login flow later
            var timezoneId = User?.FindFirst("TimeZoneId")?.Value;

            // 2) Fallback to browser cookie
            if (string.IsNullOrWhiteSpace(timezoneId) && Request?.Cookies != null)
            {
                Request.Cookies.TryGetValue(UserTimeZoneCookieKey, out timezoneId);
            }

            if (string.IsNullOrWhiteSpace(timezoneId))
                return TimeZoneInfo.Utc;

            if (TryFindTimeZone(timezoneId, out var tz))
                return tz;

            if (TimeZoneFallbackMap.TryGetValue(timezoneId, out var mappedId) &&
                TryFindTimeZone(mappedId, out tz))
                return tz;

            return TimeZoneInfo.Utc;
        }

        protected string ToUserTimeString(DateTimeOffset value, string format = "dd-MMM-yyyy hh:mm tt")
        {
            var userTime = TimeZoneInfo.ConvertTime(value, GetUserTimeZoneInfo());
            return userTime.ToString(format);
        }

        private static bool TryFindTimeZone(string timezoneId, out TimeZoneInfo timezoneInfo)
        {
            try
            {
                timezoneInfo = TZConvert.GetTimeZoneInfo(timezoneId);
                return true;
            }
            catch
            {
                timezoneInfo = TimeZoneInfo.Utc;
                return false;
            }
        }
        #endregion

        protected string GetUserUUID()
            {
                if (User == null)
                    return GetUserUUID();

                // Try common claim names where a UUID might be stored
                var userUuid = User.FindFirst(Constants.ClaimConstants.UUID)?.Value;

                return string.IsNullOrWhiteSpace(userUuid) ? "System" : userUuid;
            }

            /// <summary>
            /// Sets a success message in TempData
            /// </summary>
            protected void SetSuccessMessage(string message)
            {
                TempData["Message"] = message;
                TempData["MessageType"] = "success";
            }

            /// <summary>
            /// Sets an error/danger message in TempData
            /// </summary>
            protected void SetErrorMessage(string message)
            {
                TempData["Message"] = message;
                TempData["MessageType"] = "danger";
            }

            /// <summary>
            /// Sets a message in TempData based on success flag
            /// </summary>
            protected void SetMessage(bool success, string message)
            {
                TempData["Message"] = message;
                TempData["MessageType"] = success ? "success" : "danger";
            }

            /// <summary>
            /// Sets an info message in TempData
            /// </summary>
            protected void SetInfoMessage(string message)
            {
                TempData["Message"] = message;
                TempData["MessageType"] = "info";
            }

            /// <summary>
            /// Sets a warning message in TempData
            /// </summary>
            protected void SetWarningMessage(string message)
            {
                TempData["Message"] = message;
                TempData["MessageType"] = "warning";
            }

            protected string GetBaseUrl()
            {
                var request = HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                return baseUrl;
            }
        /// <summary>
        /// Converts DateTimeOffset to Indian Standard Time formatted string
        /// </summary>
        protected string UserToTimeString(DateTimeOffset? dateTime)
        {
            if (!dateTime.HasValue)
                return "-";

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var indianTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.Value.UtcDateTime, tz);
                return indianTime.ToString("dd-MMM-yyyy hh:mm tt");
            }
            catch
            {
                // Fallback to UTC with IST offset
                var istTime = dateTime.Value.ToOffset(TimeSpan.FromHours(5.5));
                return istTime.ToString("dd-MMM-yyyy hh:mm tt");
            }
        }
    }
    }