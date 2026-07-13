using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndiaAdminPanel.Filters
{
    public class PathPermissionFilter : IAsyncActionFilter
    {
        private readonly IMasterPathPermissionService _pathPermissionService;

        // paths that should not be checked by permission filter
        private static readonly string[] _whitelistStartsWith = new[]
        {
            "/auth/login",
            "/auth/accessdenied",
            "/auth/forgotpassword",
            "/auth/resetpassword",
            "/assets",     // static assets
            "/lib",
            "/css",
            "/js",
            "/favicon.ico"
        };

        public PathPermissionFilter(IMasterPathPermissionService pathPermissionService)
        {
            _pathPermissionService = pathPermissionService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Explicitly skip Auth/Login action (GET + POST)
            var controller = context.ActionDescriptor.RouteValues.TryGetValue("controller", out var c) ? c : null;
            var action = context.ActionDescriptor.RouteValues.TryGetValue("action", out var a) ? a : null;

            if (string.Equals(controller, "Auth", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(action, "Login", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            var http = context.HttpContext;
            var req = http.Request;

            // normalize path (lowercase, trim trailing slash)
            var rawPath = req.Path.Value ?? "/";
            var normalized = rawPath.TrimEnd('/').ToLowerInvariant();
            if (string.IsNullOrEmpty(normalized)) normalized = "/";

            // whitelist common public paths and static files
            if (_whitelistStartsWith.Any(p => normalized.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            {
                await next();
                return;
            }

            // If not authenticated, redirect to login
            if (http.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            // Get role UUID from claim
            var roleUuid = http.User.FindFirst("RoleUUID")?.Value;
            if (string.IsNullOrWhiteSpace(roleUuid))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }

            // Check path permission
            var hasAccess = false;
            try
            {
                hasAccess = await _pathPermissionService.HasAccessToPathAsync(roleUuid, normalized);
            }
            catch
            {
                hasAccess = false;
            }

            if (!hasAccess)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }

            await next();
        }

    }
}
   
