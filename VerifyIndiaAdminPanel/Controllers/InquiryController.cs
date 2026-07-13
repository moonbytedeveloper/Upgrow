using Microsoft.AspNetCore.Mvc;
using Upgrow.Application;
using Upgrow.Application.DTO.Inquiry;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Inquiry;
using Upgrow.Infrastructure.Filters;
using UpgrowAdminPanel.Models.Inquiry;

namespace UpgrowAdminPanel.Controllers
{
    public class InquiryController : BaseController
    {
        private readonly IDataTableParser _dataTableParser;
        private readonly IInquiryGeneralService _inquiryGeneralService;
        private readonly IInquiryAgentService _inquiryAgentService;
        private readonly IInquiryWhiteLabelService _inquiryWhiteLabelService;
        private readonly IInquiryDistributorService _inquiryDistributerService;
        private readonly IInquiryCareerService _inquiryCareerService;
        private readonly IDomainResolverService _tenantDomainResolver;

        public InquiryController(
            IEncryptionService encryptionService,
            IDataTableParser dataTableParser,
            IInquiryGeneralService inquiryGeneralService,
            IInquiryAgentService inquiryAgentService,
            IInquiryWhiteLabelService inquiryWhiteLabelService,
            IInquiryDistributorService inquiryDistributerService,
            IDomainResolverService tenantDomainResolver,
            IInquiryCareerService inquiryCareerService)
            : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _dataTableParser = dataTableParser;
            _inquiryGeneralService = inquiryGeneralService;
            _inquiryAgentService = inquiryAgentService;
            _inquiryWhiteLabelService = inquiryWhiteLabelService;
            _inquiryDistributerService = inquiryDistributerService;
            _inquiryCareerService = inquiryCareerService;
            _tenantDomainResolver = tenantDomainResolver;
        }

        #region General Inquiry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened General Inquiries List", MenuName = "Inquiry_General")]
        public IActionResult ViewInquiryGeneral() => View(new InquiryGeneralStatusVM());

        [HttpPost]
        [ActivityLog(ActivityType = "View", Description = "Fetched General Inquiries List", MenuName = "Inquiry_General")]
        public Task<IActionResult> GetGeneralInquiries()
            => GetInquiryRowsAsync(_inquiryGeneralService);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Changed General Inquiry Status", MenuName = "Inquiry_General")]
        public Task<IActionResult> ChangeGeneralInquiryStatus(string uuid, bool isActive, string remark)
            => ChangeInquiryStatusAsync(_inquiryGeneralService, uuid, isActive, remark);

        #endregion

        #region Agent Inquiry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Agent Inquiries List", MenuName = "Inquiry_Agent")]
        public IActionResult ViewInquiryAgent() => View(new InquiryGeneralStatusVM());

        [HttpPost]
        [ActivityLog(ActivityType = "View", Description = "Fetched Agent Inquiries List", MenuName = "Inquiry_Agent")]
        public Task<IActionResult> GetAgentInquiries()
            => GetInquiryRowsAsync(_inquiryAgentService);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Changed Agent Inquiry Status", MenuName = "Inquiry_Agent")]
        public Task<IActionResult> ChangeAgentInquiryStatus(string uuid, bool isActive, string remark)
            => ChangeInquiryStatusAsync(_inquiryAgentService, uuid, isActive, remark);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Converted Inquiry to Agent", MenuName = "Inquiry_Agent")]
        public async Task<IActionResult> ConvertToAgent(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return Json(new { success = false, message = "Invalid inquiry." });

                await _inquiryAgentService.ConvertToAgentAsync(uuid, GetUserUUID());

                return Json(new
                {
                    success = true,
                    message = "Converted to agent successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region WhiteLabel Inquiry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened WhiteLabel Inquiries List", MenuName = "Inquiry_WhiteLabel")]
        public IActionResult ViewInquiryWhiteLabel() => View(new InquiryGeneralStatusVM());

        [HttpPost]
        [ActivityLog(ActivityType = "View", Description = "Fetched WhiteLabel Inquiries List", MenuName = "Inquiry_WhiteLabel")]
        public Task<IActionResult> GetWhiteLabelInquiries()
            => GetInquiryRowsAsync(_inquiryWhiteLabelService);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Changed WhiteLabel Inquiry Status", MenuName = "Inquiry_WhiteLabel")]
        public Task<IActionResult> ChangeWhiteLabelInquiryStatus(string uuid, bool isActive, string remark)
            => ChangeInquiryStatusAsync(_inquiryWhiteLabelService, uuid, isActive, remark);

        #endregion

        #region Distributer Inquiry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Distributer Inquiries List", MenuName = "Inquiry_Distributer")]
        public IActionResult ViewInquiryDistributor() => View(new InquiryGeneralStatusVM());

        [HttpPost]
        [ActivityLog(ActivityType = "View", Description = "Fetched Distributer Inquiries List", MenuName = "Inquiry_Distributer")]
        public Task<IActionResult> GetDistributerInquiries()
            => GetInquiryRowsAsync(_inquiryDistributerService);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Changed Distributer Inquiry Status", MenuName = "Inquiry_Distributer")]
        public Task<IActionResult> ChangeDistributerInquiryStatus(string uuid, bool isActive, string remark)
            => ChangeInquiryStatusAsync(_inquiryDistributerService, uuid, isActive, remark);

        #endregion

        #region Career Inquiry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Career Inquiries List", MenuName = "Inquiry_Career")]
        public IActionResult ViewInquiryCareer() => View(new InquiryGeneralStatusVM());

        [HttpPost]
        [ActivityLog(ActivityType = "View", Description = "Fetched Career Inquiries List", MenuName = "Inquiry_Career")]
        public Task<IActionResult> GetCareerInquiries()
            => GetInquiryRowsAsync(_inquiryCareerService);

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Changed Career Inquiry Status", MenuName = "Inquiry_Career")]
        public Task<IActionResult> ChangeCareerInquiryStatus(string uuid, bool isActive, string remark)
            => ChangeInquiryStatusAsync(_inquiryCareerService, uuid, isActive, remark);

        #endregion

        #region Helpers

        private async Task<IActionResult> GetInquiryRowsAsync(IPagedService<InquiryGeneralDto> service)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await service.GetPagedAsync(request);

                return Json(new
                {
                    draw = Request.Form["draw"].FirstOrDefault(),
                    recordsTotal = result.total,
                    recordsFiltered = result.total,
                    data = result.data.Select((x, index) => new
                    {
                        srno = request.Start + index + 1,
                        uuid = x.UUID,
                        fullName = x.FullName ?? $"{x.FName} {x.MName} {x.LName}",
                        emailId = x.EmailId,
                        phoneNo = x.PhoneNo,
                        message = x.Message,
                        isActive = x.IsActive,
                        status = x.IsActive ? "Open" : "Closed",
                        canUpdateStatus = x.IsActive,
                        remark = x.Remark,
                        actionTakenBy = x.ActionTakenBy,
                        isstatusclosed = x.IsStatusClosed,
                        isconvertedtoagent =  x.IsConvertedToAgent,
                        salesExperience = x.SalesExperience,
                        salesExperienceDescription = x.SalesExperienceDescription,
                        hasExistingClients = x.HasExistingClients,

                        companyName = x.CompanyName,
                        companyWebsite = x.CompanyWebsite,
                        businessType = x.BusinessType,
                        preferredDomainName = x.PreferredDomainName,
                        expectedRetailers = x.ExpectedRetailers,

                        stateUUID = x.StateUUID,
                        cityUUID = x.CityUUID,

                        jobPosition = x.JobPosition,
                        experience = x.Experience,
                        qualification = x.Qualification,
                        resume = x.Resume
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

        private async Task<IActionResult> ChangeInquiryStatusAsync(
            IInquiryStatusUpdatable service,
            string uuid,
            bool isActive,
            string remark)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return Json(new { success = false, message = "Invalid inquiry." });

                if (string.IsNullOrWhiteSpace(remark))
                    return Json(new { success = false, message = "Remark is required." });

                await service.UpdateInquiryStatusAsync(uuid, isActive, remark.Trim(), GetUserUUID());

                return Json(new
                {
                    success = true,
                    message = "Status updated successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}   