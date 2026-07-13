using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VerifyIndia.Application.IServices;
using VerifyIndia.Domain.Common;
using VerifyIndia.Infrastructure;

namespace UpgrowAdminPanel.Controllers
{
    public class ActivityLogsController : BaseController
    {
        private readonly IActivityLogsService _activityLogsService;
        private readonly IDomainResolverService _domainResolverService;

        public ActivityLogsController(
            IEncryptionService encryptionService,
            IDataTableParser dataTableParser,
            IActivityLogsService activityLogsService,
            IDomainResolverService domainResolverService)
            : base(dataTableParser, domainResolverService, encryptionService)
        {
            _activityLogsService = activityLogsService;
            _domainResolverService = domainResolverService;
        }

        [HttpPost]
        public async Task<IActionResult> GetByMenu(string menuName)
        {
            var draw = Request.Form["draw"].FirstOrDefault() ?? "1";
            var start = int.TryParse(Request.Form["start"], out var s) ? s : 0;
            var length = int.TryParse(Request.Form["length"], out var l) ? l : 10;
            var search = Request.Form["search[value]"].FirstOrDefault()?.Trim();

            // Create pagination
            var pagination = new PaginationParams
            {
                PageNumber = (start / length) + 1,
                PageSize = length
            };

            // Get data from service
            var result = await _activityLogsService.GetPagedAsync(menuName, search, pagination);
            /*var totalCount = await _activityLogsService.GetTotalCountAsync();*/

            // Format response
            var data = result.Items.Select(x => new
            {
                createdat = UserToTimeString(x.CreatedAt),
                useruuid = x.EmployeeName, // Employee name instead of UUID
                activitytype = x.ActivityType,
                description = x.Description,
                pageurl = x.PageUrl,
                ipaddress = x.IPAddress
            }).ToList();

            return Json(new
            {
                draw,
                 
                recordsFiltered = result.TotalCount,
                data
            });
        }
    }
}