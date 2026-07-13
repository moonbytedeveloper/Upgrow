using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;
using VerifyIndia.Application;
using VerifyIndia.Application.Commands.Api;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.Commands.WL;
using VerifyIndia.Application.Commands.WL.Master;
using VerifyIndia.Application.DTO.DropDown;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.DTO.WL;
using VerifyIndia.Application.DTO.WL.Master;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.DTOs.Master;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.IServices.WL.Master;
using VerifyIndia.Application.Services;
using VerifyIndia.Application.Services.WL;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;
using VerifyIndia.Infrastructure.Filters;
using UpgrowAdminPanel.Models.Master;
using UpgrowAdminPanel.Models.Tenant;
using UpgrowAdminPanel.Models.WL;

namespace UpgrowAdminPanel.Controllers
{
    [ActivityLog]
    public class TenantController : BaseController
    {
        private readonly IWLSocialMediaService _socialMediaService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment _environment;
        private readonly IDataTableParser _dataTableParser;
        private readonly IMasterRepository<Tenant> _tenantRepository;
        private readonly IWLMasterBannerService _masterBannerService;
        private readonly IWLMasterCompanyBasicDataService _companyBasicDataService;
        private readonly ITenantService _tenantService;
        private readonly IWLTenantService _wltenantService;
        private readonly IWLTenantDomainService _tenantDomainService;
        private readonly IWLClientsService _clientsService;
        private readonly IWLMasterCmsService _masterCmsService;
        private readonly IWLTestimonialService _testimonialService;
        private readonly IDomainResolverService _domainResolverService;
        private readonly IWLMasterPermissionGroupService _wlpermissionGroupService;
        public TenantController(
            IEncryptionService encryptionService,
            IWLSocialMediaService socialMediaService,
            IFileUploadService fileUploadService,
            IWebHostEnvironment environment,
            IDataTableParser dataTableParser,
            IMasterRepository<Tenant> tenantRepository,
            IWLTenantService wltenantService,
            IWLTenantDomainService tenantDomainService,
            IWLMasterBannerService masterBannerService,
            IWLClientsService clientsService,
            IWLMasterCmsService masterCmsService,
            IWLTestimonialService testimonialService,
            IDomainResolverService domainResolverService,
            ITenantService tenantService,
            IWLMasterPermissionGroupService wlMasterPermissionGroupService,
            IWLMasterCompanyBasicDataService companyBasicDataService) : base(dataTableParser, domainResolverService, encryptionService)
        {
            _fileUploadService = fileUploadService;
            _environment = environment;
            _socialMediaService = socialMediaService;
            _dataTableParser = dataTableParser;
            _tenantRepository = tenantRepository;
            _masterBannerService = masterBannerService;
            _companyBasicDataService = companyBasicDataService;
            _wltenantService = wltenantService;
            _tenantDomainService = tenantDomainService;
            _clientsService = clientsService;
            _masterCmsService = masterCmsService;
            _testimonialService = testimonialService;
            _wlpermissionGroupService = wlMasterPermissionGroupService;
            _domainResolverService = domainResolverService;
            _tenantService = tenantService;
        }
        
        #region helpermethod
        protected IActionResult DataTableResponse<TDto>(
       DataTableRequest request,
       PagedResult<TDto> result,
       Func<TDto, int, Dictionary<string, object>> map)
        {
            var srNo = request.Start + 1;

            return Json(new
            {
                draw = Request.Form["draw"].FirstOrDefault(),
                recordsTotal = result.TotalCount,
                recordsFiltered = result.TotalCount,
                data = result.Items.Select(dto => map(dto, srNo++))
            });
        }

        protected IActionResult DataTableError(Exception ex)
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
        private async Task<string?> GetCompanyNameAsync()
        {
            try
            {
                // Prefer tenant identifier resolved from request host (already stored sanitized in DB)
                var tenantFromHost = await ResolveCompanyNameFromRequestAsync();
                if (!string.IsNullOrWhiteSpace(tenantFromHost))
                    return tenantFromHost.Trim();

                return null; // FileUploadService will fallback to "Unknown"
            }
            catch
            {
                return null;
            }
        }
        protected async Task<List<SelectListItem>> BuildTenantDropdownAsync(
    List<MasterDropDownDto> tenants,
    int? selectedId = null)
        {
            return tenants
                .Select(t => new SelectListItem
                {
                    Value = t.UUID,
                    Text = t.Title,
                    Selected = selectedId.HasValue && t.UUID == selectedId.ToString()
                })
                .ToList();
        }
        private async Task<List<SelectListItem>> GetTenantSelectListAsync(int? selectedTenantId = null)
        {
            var tenants = await _tenantRepository.GetAllActiveAsync();

            return tenants
                .Where(t => t.IsActive == true && t.IsPlatformOwner == false)
                .OrderBy(t => t.TenantName)
                .Select(t => new SelectListItem
                {
                    Value = ((int)t.Id).ToString(),
                    Text = t.TenantName,
                    Selected = selectedTenantId.HasValue && (int)t.Id == selectedTenantId.Value
                })
                .ToList();
        }
        #endregion

        #region Social Media      

        // Developed by Krishna  (31-03-2026)
        

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Social Media List", MenuName = "WL_MasterSocialMedia")]
        public async Task<IActionResult> ViewSocialMedia()
        {
            
            var vm = new SocialMediaVM
            {
                Tenants = await BuildTenantDropdownAsync(await _socialMediaService.GetTenantDropdownAsync())
               // Tenants = await GetTenantSelectListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetSocialMedia([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _socialMediaService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["tenantName"] = dto.TenantName,
                    ["sequenceno"] = dto.DisplayOrder,
                    ["platformName"] = dto.PlatformName,
                    ["iconURL"] = dto.IconURL,
                    ["profileURL"] = dto.ProfileURL,
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSocialMedia), "social media"),
                    ["srno"] = srNo++
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Social Media", MenuName = "WL_MasterSocialMedia")]
        public async Task<IActionResult> AddSocialMedia()
        {
            WLSocialMediaCommand model;

            model = new WLSocialMediaCommand { IsActive = true };
            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            // ViewBag.Tenants = await GetTenantSelectListAsync(model.TenantId > 0 ? model.TenantId : null);
            return View("AddSocialMedia", model);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Social Media", MenuName = "WL_MasterSocialMedia")]
        public async Task<IActionResult> EditSocialMedia(string? uuid)
        {
            WLSocialMediaCommand model;

            if (string.IsNullOrWhiteSpace(uuid))
            {
                model = new WLSocialMediaCommand { IsActive = true };
            }
            else
            {
                var dto = await _socialMediaService.GetByUuidAsync(uuid);
                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(nameof(ViewSocialMedia));
                }
                var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);
                model = new WLSocialMediaCommand
                {
                    UUID = dto.UUID,
                    TenantId = dto.TenantId,
                    TenantName = tenant?.TenantName,
                    DisplayOrder = dto.DisplayOrder,
                    PlatformName = dto.PlatformName,
                    IconURL = dto.IconURL,
                    ProfileURL = dto.ProfileURL,
                    IsActive = dto.IsActive
                };
            }

            model.Tenants = await GetTenantSelectListAsync(model.TenantId);

            return View("AddSocialMedia", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Social Media Form", MenuName = "WL_MasterSocialMedia")]
        public async Task<IActionResult> AddSocialMedia(WLSocialMediaCommand command)
        {
            if (!ModelState.IsValid)
            {
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddSocialMedia", command);
            }

            try
            {
                var existing = await _socialMediaService.GetByUuidAsync(command.UUID);

                if (existing != null)
                {
                    command.TenantId = existing.TenantId;
                }

                await _socialMediaService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());
                SetSuccessMessage(string.IsNullOrWhiteSpace(command.UUID)
                    ? "Social Media added successfully!"
                    : "Social Media updated successfully!");

                return RedirectToAction(nameof(ViewSocialMedia));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddSocialMedia", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Social Media Status", MenuName = "Master_SocialMedia")]
        public Task<IActionResult> ToggleSocialMedia(string uuid)
                    => ToggleActiveAsync<WLSocialMediaDto, WLSocialMediaCommand>(uuid, _socialMediaService, "Social Media URL");
        #endregion

        #region Tenant      
        // Developed by Krishna  (01-04-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Tenant List", MenuName = "Master_Tenant")]
        public IActionResult ViewTenant() => View();

        [HttpPost]
        public Task<IActionResult> GetTenant()
        => GetPagedDataAsync<WLTenantDto, WLTenantCommand>(_wltenantService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.TenantName,
            ["identifier"] = dto.Identifier,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleTenant), "tenant")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Tenant", MenuName = "Master_Tenant")]
        public IActionResult AddTenant() => View("AddTenant", new WLTenantCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Tenant", MenuName = "Master_Tenant")]
        public Task<IActionResult> EditTenant(string? uuid)
            => EditMasterAsync(
                uuid,
                _wltenantService,
                () => new WLTenantCommand { IsActive = true },
                dto => new WLTenantCommand
                {
                    UUID = dto.UUID,
                    TenantName = dto.TenantName,
                    Identifier = dto.Identifier,
                    IsPlatformOwner = dto.IsPlatformOwner,
                    IsActive = dto.IsActive
                },
                "AddTenant",
                nameof(ViewTenant));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Tenant Form", MenuName = "Master_Tenant")]
        public Task<IActionResult> AddTenant(WLTenantCommand command)
            => SaveMasterAsync(
                command,
                _wltenantService,
                "Tenant",
                "AddTenant",
                nameof(ViewTenant));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Tenant Status", MenuName = "Master_Tenant")]
        public Task<IActionResult> ToggleTenant(string uuid)
            => ToggleActiveAsync<WLTenantDto, WLTenantCommand>(uuid, _wltenantService, "Tenant");
        #endregion

        #region Tenant Domain
        // Developed By : Developed by Krishna  (01-04-2026)

       
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Tenant Domain List", MenuName = "Master_TenantDomain")]
        public IActionResult ViewTenantDomain() => View();

        [HttpPost]
        public Task<IActionResult> GetTenantDomain()
        => GetPagedDataAsync<WLTenantDomainDto, WLTenantDomainCommand>(_tenantDomainService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["domain"] = dto.Domain,
            ["domaintype"] = dto.DomainType,
            ["tenant"] = !string.IsNullOrWhiteSpace(dto.TenantName) ? dto.TenantName : dto.TenantId.ToString(),
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleTenantDomain), "tenant domain")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Tenant Domain", MenuName = "Master_TenantDomain")]
        public async Task<IActionResult> AddTenantDomain()
        {
            var vm = new TenantDomainVM
            {
                TenantList = await GetTenantSelectListAsync()
            };

            vm.TenantDomain = new WLTenantDomainCommand { IsActive = true };
            return View("AddTenantDomain", vm);
            
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Tenant Domain", MenuName = "Master_TenantDomain")]
        public async Task<IActionResult> EditTenantDomain(string? uuid)
        {
            // Use numeric-id select list so TenantId (int) binds correctly
            var vm = new TenantDomainVM
            {
                TenantList = await GetTenantSelectListAsync()
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.TenantDomain = new WLTenantDomainCommand { IsActive = true };
                return View("AddTenantDomain", vm);
            }

            var dto = await _tenantDomainService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewTenantDomain));
            }

            vm.TenantDomain = new WLTenantDomainCommand
            {
                UUID = dto.UUID,
                TenantId = dto.TenantId,
                Domain = dto.Domain,
                DomainType = dto.DomainType,
                IsActive = dto.IsActive,
                IsPrimary = dto.IsPrimary
            };

            // mark selected tenant in list
            vm.TenantList = await GetTenantSelectListAsync(vm.TenantDomain.TenantId > 0 ? vm.TenantDomain.TenantId : null);

            return View("AddTenantDomain", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Tenant Domain Form", MenuName = "Master_TenantDomain")]
        public async Task<IActionResult> AddTenantDomain(TenantDomainVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.TenantDomain, nameof(vm.TenantDomain)))
            {
                // reload tenant list using numeric ids so binding works on re-render
                vm.TenantList = await GetTenantSelectListAsync();
                return View("AddTenantDomain", vm);
            }

            try
            {
                await _tenantDomainService.SaveAsync(vm.TenantDomain, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.TenantDomain.UUID)
                    ? "Tenant Domain added successfully!"
                    : "Tenant Domain updated successfully!");

                return RedirectToAction(nameof(ViewTenantDomain));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error using numeric ids
                vm.TenantList = await GetTenantSelectListAsync();
                return View("AddTenantDomain", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Tenant Domain Status", MenuName = "Master_TenantDomain")]
        public Task<IActionResult> ToggleTenantDomain(string uuid)
            => ToggleActiveAsync<WLTenantDomainDto, WLTenantDomainCommand>(uuid, _tenantDomainService, "Tenant Domain");
        #endregion   

        #region WLBanner
        // Developed by Dixita  (01-04-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Banner List", MenuName = "Master_Banner")]
        public async Task<IActionResult> ViewBanner()
        {
            var vm = new TenantVm
            {
             Tenants =  await BuildTenantDropdownAsync(await _masterBannerService.GetTenantDropdownAsync())
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> GetBanners([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _masterBannerService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["maintitle"] = dto.MainTitle,
                    ["tenant"] = dto.TenantName,
                    ["subtitle"] = dto.SubTitle,
                    ["sequence"] = dto.SequenceNo,
                    ["buttontext"] = dto.ButtonText,
                    ["buttonurl"] = dto.ButtonURL,
                    ["image"] = !string.IsNullOrEmpty(dto.BannerImage)
? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.BannerImage)}' style='width:80px;height:50px;object-fit:cover;border-radius:4px;' />"
: "",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleBanner), "banner"),

                    ["srno"] = srNo++
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }

        // Toggle IsActive
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Banner Status", MenuName = "Master_Banner")]
        public async Task<IActionResult> ToggleBanner(string uuid)
        {
            return await ToggleActiveAsync(uuid, _masterBannerService, "Banner");
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Banner", MenuName = "Master_Banner")]
        public async Task<IActionResult> EditBanner(string? uuid)
        {
            WLMasterBannerCommand model;

            if (string.IsNullOrWhiteSpace(uuid))
            {
                // Add new banner
                model = new WLMasterBannerCommand { IsActive = true };
            }
            else
            {
                // Edit existing banner
                var dto = await _masterBannerService.GetByUuidAsync(uuid);
                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(nameof(ViewBanner)); 
                }
                var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);

                model = new WLMasterBannerCommand
                {
                    UUID = dto.UUID,
                    BannerImage = dto.BannerImage,
                    OptionalTitle = dto.OptionalTitle,
                    SequenceNo = dto.SequenceNo,
                    TenantName = tenant?.TenantName,
                    MainTitle = dto.MainTitle,
                    SubTitle = dto.SubTitle,
                    ButtonText = dto.ButtonText,
                    ButtonURL = dto.ButtonURL,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.BannerImage),
                    TenantId = dto.TenantId,
                    IsActive = dto.IsActive
                };
            }
            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            return View("AddBanner", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Banner Form", MenuName = "Master_Banner")]
        public async Task<IActionResult> AddBanner(WLMasterBannerCommand command)
        {

            if (command.BannerImage == null && command.Image == null)
            {
                ModelState.AddModelError("Command.Image", "Required!");
            }
            if (!ModelState.IsValid)
            {
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddBanner", command);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var existing = await _masterBannerService.GetByUuidAsync(command.UUID);

                if (existing != null)
                {
                    command.TenantId = existing.TenantId; 
                }
                else
                {
                    SetErrorMessage("Banner not found!");
                    return RedirectToAction(nameof(ViewBanner));
                }
                if (command.Image != null)
                {

                    command.BannerImage = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        await GetCompanyNameAsync(),
                        "banner",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.BannerImage);
                }

                await _masterBannerService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());
                SetSuccessMessage(string.IsNullOrWhiteSpace(command.UUID)
                    ? "Banner added successfully!"
                    : "Banner updated successfully!");

                return RedirectToAction(nameof(ViewBanner));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddBanner", command);
            }
        }

        #endregion

        #region Company Basic Data      

        // Developed by Dixita  (01-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Company Basic Data List", MenuName = "Master_CompanyBasicData")]
        public async Task<IActionResult> ViewCompanyData()
        {
            var vm = new TenantVm
            {
                Tenants = await BuildTenantDropdownAsync(await _companyBasicDataService.GetTenantDropdownAsync())
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> GetCompanyData([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _companyBasicDataService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["tenantname"] = dto.TenantName,
                    ["compname"] = dto.CompName,
                    ["email"] = dto.EmailId,
                    ["phone"] = dto.Phone,
                    ["address"] = dto.Address,
                    ["websitelogo"] = !string.IsNullOrEmpty(dto.WebsiteLogo)
                        ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.WebsiteLogo)}' style='width:80px;height:50px;object-fit:cover;border-radius:4px;' />"
                        : "",
                    ["footerlogo"] = !string.IsNullOrEmpty(dto.FooterLogo)
                        ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.FooterLogo)}' style='width:80px;height:50px;object-fit:cover;border-radius:4px;' />"
                        : "",
                    ["stickylogo"] = !string.IsNullOrEmpty(dto.StickyLogo)
                        ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.StickyLogo)}' style='width:80px;height:50px;object-fit:cover;border-radius:4px;' />"
                        : "",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCompanyData), "Company Data"),
                    ["srno"] = srNo
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Company Basic Data", MenuName = "Master_CompanyBasicData")]
        public async Task<IActionResult> EditCompanyData(string? uuid)
        {
            WLMasterCompanyBasicDataCommand model;

            if (string.IsNullOrWhiteSpace(uuid))
            {
                model = new WLMasterCompanyBasicDataCommand { IsActive = true };
            }
            else
            {
                var dto = await _companyBasicDataService.GetByUuidAsync(uuid);
                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(nameof(ViewCompanyData));
                }
                var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);
                model = new WLMasterCompanyBasicDataCommand
                {
                    UUID = dto.UUID,
                    TenantId = dto.TenantId,
                    TenantName = tenant?.TenantName,
                    CompName = dto.CompName,
                    EmailId = dto.EmailId,
                    Phone = dto.Phone,
                    Address = dto.Address,
                    WebsiteLogo = dto.WebsiteLogo,
                    FooterLogo = dto.FooterLogo,
                    GoogleMapIframe = dto.GoogleMapIframe,
                    StickyLogo = dto.StickyLogo,
                    websiteUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.WebsiteLogo),
                    stickylogoUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.StickyLogo),
                    footerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.FooterLogo),
                    IsActive = dto.IsActive
                };
            }

            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            return View("AddCompanyData", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Company Basic Data Form", MenuName = "Master_CompanyBasicData")]
        public async Task<IActionResult> AddCompanyData(WLMasterCompanyBasicDataCommand command)
        {
            if (command.WebsiteLogo == null && command.WebsiteLogoImage == null)
            {
                ModelState.AddModelError("WebsiteLogoImage", "Website logo is required!");
            }
            if (command.StickyLogo == null && command.StickyLogoImage == null)
            {
                ModelState.AddModelError("StickyLogoImage", "Sticky logo is required!");
            }
            if (command.FooterLogo == null && command.FooterLogoImage == null)
            {
                ModelState.AddModelError("FooterLogoImage", "Footer logo is required!");
            }

            if (!ModelState.IsValid)
            {
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddCompanyData", command);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                // 🔥 LIKE BANNER: check existing record
                var existing = await _companyBasicDataService.GetByUuidAsync(command.UUID);

                if (existing != null)
                {
                    command.TenantId = existing.TenantId;
                }

                // Upload Website Logo
                if (command.WebsiteLogoImage != null)
                {
                    command.WebsiteLogo = await _fileUploadService.SaveFileAsync(
                        command.WebsiteLogoImage,
                        await GetCompanyNameAsync(),
                        "websitelogo",
                        allowedExtensions);
                    command.websiteUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.WebsiteLogo);
                }
                else if (existing != null)
                {
                    command.WebsiteLogo = existing.WebsiteLogo;
                }

                // Upload Sticky Logo
                if (command.StickyLogoImage != null)
                {
                    command.StickyLogo = await _fileUploadService.SaveFileAsync(
                        command.StickyLogoImage,
                        await GetCompanyNameAsync(),
                        "stickylogo",
                        allowedExtensions);
                    command.stickylogoUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.StickyLogo);
                }
                else if (existing != null)
                {
                    command.StickyLogo = existing.StickyLogo;
                }

                // Upload Footer Logo
                if (command.FooterLogoImage != null)
                {
                    command.FooterLogo = await _fileUploadService.SaveFileAsync(
                        command.FooterLogoImage,
                        await GetCompanyNameAsync(),
                        "footerlogo",
                        allowedExtensions);
                    command.footerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.FooterLogo);
                }
                else if (existing != null)
                {
                    command.FooterLogo = existing.FooterLogo;
                }

                await _companyBasicDataService.SaveAsync(
                    command,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress()
                );

                SetSuccessMessage(string.IsNullOrWhiteSpace(command.UUID)
                    ? "Data added successfully!"
                    : "Data updated successfully!");

                return RedirectToAction(nameof(ViewCompanyData));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddCompanyData", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Company Data Status", MenuName = "Master_CompanyBasicData")]
        public Task<IActionResult> ToggleCompanyData(string uuid)
           => ToggleActiveAsync<WLMasterCompanyBasicDataDto, WLMasterCompanyBasicDataCommand>(uuid, _companyBasicDataService, "Company Data");
        #endregion

        #region Clients      

        // Developed by Krishna  (02-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Clients List", MenuName = "Master_Clients")]
        public async Task<IActionResult> ViewClients()
        {
            
            var vm = new ClientsVM
            {
                Tenants = await BuildTenantDropdownAsync(await _clientsService.GetTenantDropdownAsync())
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetClients([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _clientsService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["tenantid"] = !string.IsNullOrEmpty(dto.TenantName) ? dto.TenantName : dto.TenantId.ToString(),

                    ["name"] = dto.Name,
                    ["iconimage"] = !string.IsNullOrEmpty(dto.IconImage)
                        ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.IconImage)}' style='width:50px;height:50px;object-fit:cover;border-radius:4px;' />"
                        : "",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleClients), "clients"),
                    ["srno"] = srNo
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Clients", MenuName = "WL_MasterClients")]
        public async Task<IActionResult> AddClients() 
        {
            WLClientsCommand model;

            model = new WLClientsCommand { IsActive = true };
            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            // ViewBag.Tenants = await GetTenantSelectListAsync(model.TenantId > 0 ? model.TenantId : null);
            return View("AddClients", model);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Clients", MenuName = "WL_MasterClients")]
        public async Task<IActionResult> EditClients(string? uuid)
        {
            WLClientsCommand model;

            if (string.IsNullOrWhiteSpace(uuid))
            {
                model = new WLClientsCommand { IsActive = true };
            }
            else
            {
                var dto = await _clientsService.GetByUuidAsync(uuid);
                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(nameof(ViewClients));
                }
                var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);

                model = new WLClientsCommand
                {
                    UUID = dto.UUID,
                    TenantId = dto.TenantId,
                    TenantName = tenant?.TenantName,
                    Name = dto.Name,
                    IconImage = dto.IconImage,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.IconImage),
                    IsActive = dto.IsActive
                };
            }

            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            return View("AddClients", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Clients Form", MenuName = "WL_MasterClients")]
        public async Task<IActionResult> AddClients(WLClientsCommand command)
        {
            // Validate logos
            if (command.IconImage == null && command.Image == null)
            {
                ModelState.AddModelError("Image", "Icon is required!");
            }
           
            if (!ModelState.IsValid)
            {
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddClients", command);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var existing = await _clientsService.GetByUuidAsync(command.UUID);


                if (existing != null)
                {
                    command.TenantId = existing.TenantId;
                }
                else
                {
                    SetErrorMessage("Data not found!");
                    return RedirectToAction(nameof(ViewClients));
                }

                // Handle Website Logo Upload
                if (command.Image != null)
                {
                    command.IconImage = await _fileUploadService.SaveFileAsync(
                        command.Image,
                         await GetCompanyNameAsync(),
                        "clients",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.IconImage);
                }

                await _clientsService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());
                SetSuccessMessage(string.IsNullOrWhiteSpace(command.UUID)
                    ? "Data added successfully!"
                    : "Data updated successfully!");

                return RedirectToAction(nameof(ViewClients));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddClients", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Clients Status", MenuName = "Master_Clients")]
        public Task<IActionResult> ToggleClients(string uuid)
           => ToggleActiveAsync<WLClientsDto, WLClientsCommand>(uuid, _clientsService, "Clients");
        #endregion

        #region CMS Master     

        // Developed by Dixita  (02-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened CMS List", MenuName = "Master_CMS")]
        public async Task<IActionResult> ViewCMS()
        {
            var vm = new TenantVm
            {
                Tenants = await BuildTenantDropdownAsync(await _masterCmsService.GetTenantDropdownAsync())
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetCMS([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _masterCmsService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["tenantid"] = !string.IsNullOrEmpty(dto.TenantName) ? dto.TenantName : dto.TenantId.ToString(),
                    ["pagename"] = dto.PageTitle,
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCMS), "CMS"),
                    ["srno"] = srNo
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }
        // Load tenant-wise CMS options
        // Edit CMS
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit CMS", MenuName = "Master_CMS")]
        public async Task<IActionResult> SaveCMS(string uuid, int tenantId)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                SetErrorMessage("Please select a page to edit.");
                return RedirectToAction(nameof(ViewCMS), new { tenantId });
            }

            var dto = await _masterCmsService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewCMS), new { tenantId });
            }
            var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);
            var vm = new WLMasterCMSCommand
            {
                UUID = dto.UUID,
                PageTitle = dto.PageTitle,
                Description = dto.Description,
                UploadImage = dto.UploadImage,
                ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.UploadImage),
                IsActive = dto.IsActive,
                TenantId = dto.TenantId,
                TenantName = tenant?.TenantName
            };

            return View("SaveCMS", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Save CMS Form", MenuName = "Master_CMS")]
        public async Task<IActionResult> SaveCMS(WLMasterCMSCommand model)
        {
            if (model.UploadImage == null && model.Image == null)
            {
                ModelState.AddModelError("model.Image", "Image is required!");
            }

            if (!ModelState.IsValid)
            {
                return View("SaveCMS", model);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var existing = await _masterCmsService.GetByUuidAsync(model.UUID);

                if (existing != null)
                {
                    model.TenantId = existing.TenantId;
                    model.PageTitle = existing.PageTitle;
                }
                else
                {
                    SetErrorMessage("Banner not found!");
                    return RedirectToAction(nameof(ViewBanner));
                }
                if (model.Image != null)
                {
                    existing.UploadImage = await _fileUploadService.SaveFileAsync(model.Image, await GetCompanyNameAsync(), "cms", allowedExtensions);
                }
                await _masterCmsService.SaveAsync(model, GetUserUUID(), Utils.GetLocalIPAddress());
                SetSuccessMessage("CMS content updated successfully!");
                return RedirectToAction(nameof(ViewCMS));
            }
            catch (Exception ex)
            {
                SetErrorMessage($"Error saving CMS content: {ex.Message}");
                return View("SaveCMS", model);
            }
        }

        [HttpPost] 
        [ActivityLog(ActivityType = "Update", Description = "Toggle CMS Status", MenuName = "Master_CMS")]
        public Task<IActionResult> ToggleCMS(string uuid)
           => ToggleActiveAsync<WLMasterCMSDto, WLMasterCMSCommand>(uuid, _masterCmsService, "CMS");
        #endregion

        #region Testimonial      

        // Developed by Krishna  (02-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Testimonial List", MenuName = "WL_MasterTestimonial")]
        public async Task<IActionResult> ViewTestimonial()
        {
            var vm = new TestimonialVM
            {
                Tenants = await BuildTenantDropdownAsync(await _testimonialService.GetTenantDropdownAsync())
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetTestimonial([FromQuery] string? tenantUuid)
        {
            try
            {
                var request = _dataTableParser.ParseRequest();
                var result = await _testimonialService.GetPagedByTenantAsync(request, tenantUuid);

                return DataTableResponse(request, result, (dto, srNo) => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["tenantName"] = !string.IsNullOrEmpty(dto.TenantName) ? dto.TenantName : dto.TenantId.ToString(),
                    ["customerName"] = dto.CustomerName,
                    ["companyName"] = dto.CompanyName,
                    ["comment"] = dto.Comment,

                    //["sequenceNo"] = dto.SequenceNo,
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleTestimonial), "testimonial"),

                    ["srno"] = srNo
                });
            }
            catch (Exception ex)
            {
                return DataTableError(ex);
            }
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Testimonial", MenuName = "WL_MasterTestimonial")]
        public async Task<IActionResult> AddTestimonial()
        {
            WLTestimonialCommand model;

            model = new WLTestimonialCommand { IsActive = true };
            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            // ViewBag.Tenants = await GetTenantSelectListAsync(model.TenantId > 0 ? model.TenantId : null);
            return View("AddTestimonial", model);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Testimonial", MenuName = "WL_MasterTestimonial")]
        public async Task<IActionResult> EditTestimonial(string? uuid)
        {
            WLTestimonialCommand model;

            if (string.IsNullOrWhiteSpace(uuid))
            {
                model = new WLTestimonialCommand { IsActive = true };
            }
            else
            {
                var dto = await _testimonialService.GetByUuidAsync(uuid);
                if (dto == null)
                {
                    SetErrorMessage("Record not found!");
                    return RedirectToAction(nameof(ViewTestimonial));
                }
                var tenant = await _tenantService.GetTenantNameByIdAsync(dto.TenantId);

                model = new WLTestimonialCommand
                {
                    UUID = dto.UUID,
                    Comment = dto.Comment,
                    SequenceNo = dto.SequenceNo,
                    TenantName = tenant?.TenantName,
                    CompanyName = dto.CompanyName,
                    CustomerName = dto.CustomerName,
                    Star = dto.Star,
                    FilePath = dto.FilePath,
                    ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.FilePath),
                    TenantId = dto.TenantId,
                    IsActive = dto.IsActive
                };
            }

            model.Tenants = await GetTenantSelectListAsync(model.TenantId);
            return View("AddTestimonial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Testimonial Form", MenuName = "WL_MasterTestimonial")]
        public async Task<IActionResult> AddTestimonial(WLTestimonialCommand command)
        {
            if (!ModelState.IsValid)
            {
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddTestimonial", command);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var existing = await _testimonialService.GetByUuidAsync(command.UUID);

                if (existing != null)
                {
                    command.TenantId = existing.TenantId;
                }
                else
                {
                    SetErrorMessage("Testimonial not found!");
                    return RedirectToAction(nameof(ViewTestimonial));
                }

                if (command.Image != null)
                {
                    command.FilePath = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        await GetCompanyNameAsync(),
                        "Testimonial",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.FilePath);
                }
                await _testimonialService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());
                SetSuccessMessage(string.IsNullOrWhiteSpace(command.UUID)
                    ? "Testimonial added successfully!"
                    : "Testimonial updated successfully!");

                return RedirectToAction(nameof(ViewTestimonial));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.Tenants = await GetTenantSelectListAsync(command.TenantId);
                return View("AddTestimonial", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggle Testimonial Status", MenuName = "WL_MasterTestimonial")]
        public Task<IActionResult> ToggleTestimonial(string uuid)
                    => ToggleActiveAsync<WLTestimonialDto, WLTestimonialCommand>(uuid, _testimonialService, "Testimonial");
        #endregion

        #region Permission Group
        //Developed by : Krishna(20-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Permission Group List", MenuName = "WL_MasterPermissionGroup")]
        public IActionResult ViewPermissionGroup() => View();

        [HttpPost]
        public Task<IActionResult> GetPermissionGroupList()
        => GetPagedDataAsync<WLMasterPermissionGroupDto, WLMasterPermissionGroupCommand>(_wlpermissionGroupService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(TogglePermissionGroups), "Permission Group")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Permission Group", MenuName = "WL_MasterPermissionGroup")]
        public IActionResult AddPermissionGroup()
        {
            return View("AddPermissionGroup", new WLMasterPermissionGroupCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Permission Group", MenuName = "WL_MasterPermissionGroup")]
        public Task<IActionResult> EditPermissionGroup(string? uuid)
            => EditMasterAsync(
                uuid,
                _wlpermissionGroupService,
                () => new WLMasterPermissionGroupCommand { IsActive = true },
                dto => new WLMasterPermissionGroupCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "AddPermissionGroup",
                nameof(ViewPermissionGroup));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Permission Group", MenuName = "WL_MasterPermissionGroup")]
        public Task<IActionResult> AddPermissionGroup(WLMasterPermissionGroupCommand command)
            => SaveMasterAsync(
                command,
                _wlpermissionGroupService,
                "Permission Group",
                "AddPermissionGroup",
                nameof(ViewPermissionGroup));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Permission Group Status", MenuName = "WL_MasterPermissionGroup")]
        public Task<IActionResult> TogglePermissionGroups(string uuid)
            => ToggleActiveAsync<WLMasterPermissionGroupDto, WLMasterPermissionGroupCommand>(uuid, _wlpermissionGroupService, "Permission Group");

        #endregion
    }

}
