using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using VerifyIndia.Application;
using VerifyIndia.Application.DTO;
using VerifyIndia.Application.Helper;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories.Master;
using VerifyIndia.Infrastructure;
using static VerifyIndia.Application.Constants;

namespace VerifyIndia.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ActivityLogAttribute : ActionFilterAttribute
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MenuName { get; set; } = string.Empty;

        private const string LoggedKey = "ActivityLog_Logged";

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executed = await next();

            if (executed.Exception != null)
                return;

            // Prevent duplicate logging
            if (context.HttpContext.Items.ContainsKey(LoggedKey))
                return;

            try
            {
                var actionName = context.ActionDescriptor.RouteValues["action"]?.ToString() ?? "Unknown";
                var httpMethod = context.HttpContext.Request.Method;

                // Determine if this action should be logged
                if (!ShouldLogAction(actionName, httpMethod))
                    return;

                var activityLogsLogsService = context.HttpContext.RequestServices.GetRequiredService<IActivityLogsLogsService>();
                var employeeRepository = context.HttpContext.RequestServices.GetRequiredService<IMasterEmployeeRepository>();

                var logEntry = await CreateActivityLogAsync(context, employeeRepository);

                if (logEntry != null)
                {
                    await activityLogsLogsService.AddAsync(logEntry);
                    context.HttpContext.Items[LoggedKey] = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ActivityLog Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Determines if an action should be logged
        /// </summary>
        private bool ShouldLogAction(string actionName, string httpMethod)
        {
            // If explicit config is provided on attribute, always log this action
            if (!string.IsNullOrWhiteSpace(ActivityType) ||
                !string.IsNullOrWhiteSpace(Description) ||
                !string.IsNullOrWhiteSpace(MenuName))
            {
                return true;
            }

            // SKIP all actions starting with "Get" - these are AJAX data retrieval calls
            if (actionName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
                return false;

            var skipGetActions = new[]
            {
                "ViewProfile",
                "EditProfile"
            };

            if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var skipAction in skipGetActions)
                {
                    if (actionName.Equals(skipAction, StringComparison.OrdinalIgnoreCase))
                        return false;
                }

                var lowerAction = actionName.ToLower();
                return lowerAction.Contains("view") ||
                       lowerAction.Contains("edit") ||
                       lowerAction.Contains("add") ||
                       lowerAction.Contains("change") ||
                       lowerAction.Contains("manage");
            }

            return httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<ActivityLogs> CreateActivityLogAsync(
            ActionExecutingContext context,
            IMasterEmployeeRepository employeeRepository)
        {
            var employeeCode = context.HttpContext.User?.FindFirst(ClaimConstants.EmployeeCode)?.Value;
            var userUuid = await GetUserUuidAsync(context, employeeRepository);
            var pageUrl = ExtractPageUrl(context);
            var actionName = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";

            var activityType = ActivityType;
            var description = Description;
            var menuName = MenuName;

            // Create DTO and serialize to Payload
            var data = new ActivityLogDto
            {
                UserUUID = userUuid,
                ActivityType = activityType,
                Description = description,
                MenuName = menuName,
                PageUrl = pageUrl,
                IPAddress = GetClientIpAddress(context),
                CreatedAt = DateTimeOffset.Now
            };

            var json = JsonSerializer.Serialize(data);

            return new ActivityLogs
            {
                Payload = json
            };
        }

        private async Task<string> GetUserUuidAsync(ActionExecutingContext context, IMasterEmployeeRepository employeeRepository)
        {
            // 1. Try UUID from claims (set at login)
            var claimUuid = context.HttpContext.User?.FindFirst(ClaimConstants.UUID)?.Value;
            if (!string.IsNullOrWhiteSpace(claimUuid))
                return claimUuid;

            // 2. Fallback: EmployeeCode -> DB lookup
            var employeeCode = context.HttpContext.User?.FindFirst(ClaimConstants.EmployeeCode)?.Value;
            if (string.IsNullOrWhiteSpace(employeeCode))
                return "System";

            try
            {
                var employee = await employeeRepository.GetByEmployeeCodeAsync(employeeCode);
                if (employee != null && !string.IsNullOrWhiteSpace(employee.UUID))
                    return employee.UUID;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Employee lookup error: {ex.Message}");
            }

            return "System";
        }

        private string ExtractPageUrl(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var url = $"{request.Path}{request.QueryString}";
            return url.Length > 100 ? url[..100] : url;
        }

        private string GetClientIpAddress(ActionExecutingContext context)
        {
            return context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }
}