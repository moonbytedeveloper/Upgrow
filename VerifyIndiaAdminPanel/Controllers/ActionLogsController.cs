using Microsoft.AspNetCore.Mvc;
using VerifyIndia.Application.IServices.ActionLogs;
using VerifyIndia.Domain.Common;
using VerifyIndia.Infrastructure;

namespace VerifyIndiaAdminPanel.Controllers
{
    public class ActionLogsController : Controller
    {
        private readonly IActionLogsService _actionLogsService;

        public ActionLogsController(IActionLogsService actionLogsService)
        {
            _actionLogsService = actionLogsService;
        }

        /// <summary>
        /// Gets paged action logs - optimized for performance.
        /// Handles pagination, search, and filtering server-side.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GetActionLogsPaged()
        {
            var draw = Request.Form["draw"].FirstOrDefault() ?? "1";
            var entityName = Request.Form["entityName"].ToString();
            var entityUUID = Request.Form["entityUUID"].ToString();
            var includeChildren = Request.Form["includeChildren"].ToString().ToLower() == "true";
            var lineEntityNames = Request.Form["lineEntityNames"].ToString();

            //   OPTIMIZED: Return early if required parameters are missing
            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(entityUUID))
            {
                return Json(new
                {
                    draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>()
                });
            }

            try
            {
                var start = int.TryParse(Request.Form["start"], out var s) ? s : 0;
                var length = int.TryParse(Request.Form["length"], out var l) ? l : 10;
                var search = Request.Form["search[value]"].FirstOrDefault()?.Trim();

                var pagination = new PaginationParams
                {
                    PageNumber = (start / length) + 1,
                    PageSize = length
                };

                //   OPTIMIZED: Single service call handles everything
                var result = await _actionLogsService.GetPagedByEntityAsync(
                    entityName,
                    entityUUID,
                    includeChildren,
                    lineEntityNames,
                    search,
                    pagination);

                //   OPTIMIZED: Map only current page items to response
                var data = result.Items.Select((x, i) => new
                {
                    id = x.Id,
                    srno = start + i + 1,
                    date = x.CreatedAt?.ToString("dd-MMM-yyyy HH:mm:ss") ?? "-",
                    actiontype = x.ActionType ?? "-",
                    user = x.UserName ?? "-",
                    ip = x.IPAddress ?? "-"
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
                //  OPTIMIZED: Log error but return clean response
                System.Diagnostics.Debug.WriteLine($"Error in GetActionLogsPaged: {ex.Message}");

                return Json(new
                {
                    draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>(),
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a single action log by ID for detail view.
        /// Optimized with direct ID lookup - no unnecessary filtering.
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetLogDetail(decimal logId)
        {
            //   OPTIMIZED: Validate input early
            if (logId <= 0)
            {
                return Json(new { success = false, message = "Invalid log ID." });
            }

            try
            {
                //   OPTIMIZED: Direct ID lookup - no filtering needed
                var log = await _actionLogsService.GetByIdAsync(logId);

                if (log == null)
                {
                    return Json(new { success = false, message = "Log not found." });
                }

                return Json(new { success = true, data = log });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetLogDetail: {ex.Message}");
                return Json(new { success = false, message = "Error loading log details: " + ex.Message });
            }
        }
    }
}