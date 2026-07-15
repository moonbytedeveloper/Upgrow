using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Upgrow.Application;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.Customer;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.DTO.WL.Master;
using Upgrow.Application.DTOs.Master;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Api;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Verification;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.IServices.WL.Master;
using Upgrow.Application.Services.Api;
using Upgrow.Application.Services.Master;
using Upgrow.Application.Services.Verification;
using Upgrow.Application.Services.Website;
using Upgrow.Application.Services.WL;
using Upgrow.Domain.Entities;
using Upgrow.Infrastructure.Filters;
using UpgrowAdminPanel.Models;
using UpgrowAdminPanel.Models.Api;
using UpgrowAdminPanel.Models.Master;
using UpgrowAdminPanel.Models.Website;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Upgrow.Application.Constants;

namespace UpgrowAdminPanel.Controllers
{
    public class MasterController : BaseController
    {
        private readonly IMasterBusinessIndustryService _businessIndustryService;
        private readonly IMasterOfferService _offerService;
        private readonly IMasterVerificationFeeService _verificationFeeService;
        private readonly IManageApiService _manageApiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMasterGenderService _genderService;
        private readonly IMasterIndustryService _industryService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment _environment;
        private readonly IDataTableParser _dataTableParser;
        private readonly IMasterHonorificService _honorificService;
        private readonly IMasterApiCategoryService _apiCategoryService;
        private readonly IMasterRoleService _roleService;
        private readonly IMasterPermissionService _permissionService;
        private readonly IMasterEmailCredentialService _emailCredentialService;
        private readonly IMasterEmailTemplateService _emailTemplateService;
        private readonly IMasterCountryService _countryService;
        private readonly IMasterStateService _stateService;
        private readonly IMasterCityService _cityService;
        private readonly IMasterMenuService _menuService;
        private readonly IMasterApiService _apiService;
        private readonly IMasterDepartmentService _departmentService;
        private readonly IMasterDesignationService _designationService;
        private readonly IMasterPathService _pathService;
        private readonly IMasterPathPermissionService _pathpermissionService;
        private readonly IMenuQueryService _menuQueryService;
        private readonly IMasterEmployeeService _employeeService;
        private readonly IMasterNomenclatureService _nomenclatureService;
        private readonly IProviderApisService _providerApisService;
        private readonly IApiProviderService _apiProviderService;
        private readonly IMasterBlogService _blogService;
        private readonly IMasterBlogCategoryService _blogCategoryService;
        private readonly IMasterCMSService _cmsService;
        private readonly IMasterCompanyBasicDataService _companyBasicDataService;
        private readonly IMasterPermissionGroupService _PermissionGroupService;
        private readonly ITrustedPartnersService _trustedPartnersService;
        private readonly IPasswordHasher<MasterEmployeeCommand> _passwordHasher;
        private readonly IMasterBannerService _bannerService;
        private readonly IMasterFaqService _faqService;
        private readonly IMasterFaqCategoryService _faqCategoryService;
        private readonly IServiceCategoryService _serviceCategoryService;
        private readonly IMasterTestimonialService _testimonialsService;
        private readonly IMasterCareerService _careerService;
        private readonly IMasterSocialMediaService _socialMediaService;
        private readonly IMasterDocumentService _documentService;
        private readonly IMasterFinancialYearService _financialYearService;
        private readonly IDomainResolverService _domainResolverService;
        private readonly IPasswordPolicyService _passwordPolicyService;
        private readonly IMasterDosDontsService _masterDosDontsService;
        private readonly IWLMasterPathService _WLMasterpathServices;
        private readonly IWLMasterPathPermissionService _WLMasterPathPermissionService;
        private readonly IWLMasterMenuService _wlMasterMenuService;
        private readonly IWLMasterPermissionService _wlMasterPermissionService;
        private readonly ICredentialsWhatsappServices _credentialsWhatsappServices;
        private readonly ICredentialsSMSGatewayServices _credentialsSMSGatewayServices;
        private readonly IMasterFaqSubCategoryService _subCategoryService;
        private readonly IWLMasterPermissionGroupService _wlpermissionGroupService;
        private readonly IMasterCustomerService _customerService;
        private readonly IApiEndpointService _apiEndpointService;
        private readonly IApiInfoSectionService _apiInfoSectionService;
        private readonly IMasterProgrammingLanguageService _masterProgrammingLanguageService;
        private readonly IApiProviderService _ProviderService;
        private readonly IApiProviderMappingService _apiProviderMappingService;
        private readonly IApiProviderComponentMappingService _componentMappingService;
        private readonly IProviderApisService _providerapiservice;
        private readonly IMasterSkillsService _websiteSkillsService;
        private readonly IMasterJobTypeService _websiteJobTypeService;
        private readonly IWebsiteIndustryService _websiteIndustryService;
        private readonly IMasterPolicyService _masterPolicyService;
        private readonly IWebsiteIndustryPointsService _websitepointsService;
        private readonly IWebsiteIndustryCasesService _websitecasesService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IApiInfoFieldsService _apiInfoFieldsService;
        public MasterController(
            IWebsiteIndustryCasesService websitecasesService,
            IWebsiteIndustryPointsService websitepointsService,
            IMasterBusinessIndustryService businessIndustryService,
            IAppSettingsService appSettingsService,
            IMasterVerificationFeeService verificationFeeService,
            IEncryptionService encryptionService,
            IMasterOfferService offerService,
            IManageApiService manageApiService,
            IHttpClientFactory httpClientFactory,
            IApiInfoSectionService apiInfoSectionService,
            IConfiguration configuration,
            IMasterGenderService genderService,
            IDataTableParser dataTableParser,
            IWLMasterPermissionService wlMasterPermissionService,
            IMasterDesignationService designationService,
            IMasterHonorificService honorificService,
            IMasterApiCategoryService apiCategoryService,
            IMasterIndustryService industryService,
            IFileUploadService fileUploadService,
            IWebHostEnvironment environment,
            IMasterEmailCredentialService emailCredentialService,
            IMasterEmailTemplateService emailTemplateService,
            IMasterPermissionService permissionService,
            IMasterDepartmentService departmentService,
            IMasterCountryService countryService,
            IMasterStateService stateService,
            IMasterCityService cityService,
            IMasterRoleService roleService,
            IMasterMenuService menuService,
            IMasterEmployeeService employeeService,
            IMasterPathService pathService,
            IMasterApiService masterApiService,
            IMasterNomenclatureService nomenclatureService,
            IMasterPathPermissionService pathPermissionService,
            IProviderApisService providerApisService,
            IApiProviderService apiProviderService,
            IMasterPermissionGroupService PermissionGroupService,
            IMenuQueryService menuQueryService,
            IMasterSocialMediaService socialMediaService,
            IMasterFaqService faqService,
            IMasterFaqCategoryService faqCategoryService,
            IServiceCategoryService serviceCategoryService,
            IMasterBlogService blogService,
            IMasterCMSService cmsService,
            IMasterCompanyBasicDataService companyBasicDataService,
            IMasterBlogCategoryService masterBlogCategoryService,
            IMasterTestimonialService testimonialsService,
            IMasterCareerService careerService,
            IMasterBannerService bannerService,
            IMasterDocumentService documentService,
            ITrustedPartnersService trustedPartnersService,
            IPasswordHasher<MasterEmployeeCommand> passwordHasher,
            IMasterFinancialYearService financialYearService,
            IWLMasterMenuService wlMasterMenuService,
            IMasterDosDontsService masterDosDontsService,
            IDomainResolverService domainResolverService,
            IWLMasterPathPermissionService wLMasterPathPermissionService,
            IWLMasterPathService wLMasterPathService,
            ICredentialsWhatsappServices credentialsWhatsappServices,
            ICredentialsSMSGatewayServices credentialsSMSGatewayServices,
            IWLMasterPermissionGroupService wlpermissionGroupService,
            IMasterFaqSubCategoryService subCategoryService,
            IMasterCustomerService customerService,
            IApiEndpointService apiEndpointService,
            IMasterProgrammingLanguageService masterProgrammingLanguageService,
            IApiProviderService providerService,
            IApiProviderMappingService apiProviderMappingService,
            IApiComponentsService apiComponentsService,
            IApiInfoFieldsService apiInfoFieldsService,
            IProviderApisService providerapiservice,
            IApiProviderComponentMappingService componentMappingService,
            IPasswordPolicyService passwordPolicyService,
            IMasterSkillsService websiteSkillsService,
            IMasterJobTypeService websiteJobTypeService,
            IWebsiteIndustryService websiteIndustryService,
            IMasterPolicyService masterPolicyService) : base(dataTableParser, domainResolverService, encryptionService)
        {
            _websitecasesService = websitecasesService;
            _websitepointsService = websitepointsService;
            _websiteIndustryService = websiteIndustryService;
            _businessIndustryService = businessIndustryService;
            _appSettingsService = appSettingsService;
            _manageApiService = manageApiService;
            _apiEndpointService = apiEndpointService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _genderService = genderService;
            _dataTableParser = dataTableParser;
            _honorificService = honorificService;
            _fileUploadService = fileUploadService;
            _industryService = industryService;
            _environment = environment;
            _designationService = designationService;
            _departmentService = departmentService;
            _apiCategoryService = apiCategoryService;
            _emailCredentialService = emailCredentialService;
            _emailTemplateService = emailTemplateService;
            _permissionService = permissionService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _roleService = roleService;
            _menuService = menuService;
            _employeeService = employeeService;
            _nomenclatureService = nomenclatureService;
            _pathService = pathService;
            _pathpermissionService = pathPermissionService;
            _apiService = masterApiService;
            _menuQueryService = menuQueryService;
            _providerApisService = providerApisService;
            _apiProviderService = apiProviderService;
            _PermissionGroupService = PermissionGroupService;
            _passwordHasher = passwordHasher;
            _blogService = blogService;
            _blogCategoryService = masterBlogCategoryService;
            _cmsService = cmsService;
            _companyBasicDataService = companyBasicDataService;
            _testimonialsService = testimonialsService;
            _careerService = careerService;
            _faqService = faqService;
            _bannerService = bannerService;
            _faqCategoryService = faqCategoryService;
            _serviceCategoryService = serviceCategoryService;
            _socialMediaService = socialMediaService;
            _trustedPartnersService = trustedPartnersService;
            _documentService = documentService;
            _financialYearService = financialYearService;
            _domainResolverService = domainResolverService;
            _wlMasterMenuService = wlMasterMenuService;
            _wlMasterPermissionService = wlMasterPermissionService;
            _masterDosDontsService = masterDosDontsService;
            _WLMasterPathPermissionService = wLMasterPathPermissionService;
            _WLMasterpathServices = wLMasterPathService;
            _passwordPolicyService = passwordPolicyService;
            _credentialsWhatsappServices = credentialsWhatsappServices;
            _credentialsSMSGatewayServices = credentialsSMSGatewayServices;
            _wlpermissionGroupService = wlpermissionGroupService;
            _subCategoryService = subCategoryService;
            _customerService = customerService;
            _apiProviderService = providerService;
            _apiProviderMappingService = apiProviderMappingService;
            _masterProgrammingLanguageService = masterProgrammingLanguageService;
            _providerapiservice = providerApisService;
            _componentMappingService = componentMappingService;
            _apiInfoSectionService = apiInfoSectionService;
            _apiInfoFieldsService = apiInfoFieldsService;
            _offerService = offerService;

            _websiteSkillsService = websiteSkillsService;
            _websiteJobTypeService = websiteJobTypeService;
            _masterPolicyService = masterPolicyService;
            _verificationFeeService = verificationFeeService;
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

        // Add this helper near the other private helpers (for example, after GetCompanyNameAsync)
        private string GetAbsoluteFileUrl(string? path)
        {
            try
            {
                // Ask the domain resolver first (keeps existing behaviour)
                var resolved = _domainResolverService.BuildAbsoluteUrl(path);

                // If resolver returned an absolute URL, use it
                if (!string.IsNullOrWhiteSpace(resolved) && Uri.IsWellFormedUriString(resolved, UriKind.Absolute))
                    return resolved;

                // Fallback: build using configured FileApi:BaseUrl (appsettings.json)
                var baseUrl = _configuration?["FileApi:BaseUrl"]?.TrimEnd('/');
                if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(path))
                    return resolved ?? path ?? string.Empty;

                return $"{baseUrl}/{path.TrimStart('/')}";
            }
            catch
            {
                // On any error, fall back to the raw path to avoid throwing inside the data-mapping lambda
                return path ?? string.Empty;
            }
        }





        #region Gender      

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Gender List", MenuName = "Master_Gender")]
        public IActionResult MasterViewGender() => View();

        [HttpPost]

        public Task<IActionResult> GetGender()
        => GetPagedDataAsync<MasterGenderDto, MasterGenderCommand>(_genderService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleGender), "Gender")
        });
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Gender", MenuName = "Master_Gender")]
        public IActionResult MasterAddGender()
        {
            return View("MasterAddGender", new MasterGenderCommand
            {
                IsActive = true
            });
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Gender", MenuName = "Master_Gender")]
        public Task<IActionResult> MasterEditGender(string? uuid)
            => EditMasterAsync(
                uuid,
                _genderService,
                () => new MasterGenderCommand { IsActive = true },
                dto => new MasterGenderCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortTitle = dto.ShortTitle,
                    IsActive = dto.IsActive
                },
                "MasterAddGender",
                nameof(MasterViewGender));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Gender", MenuName = "Master_Gender")]
        public Task<IActionResult> MasterAddGender(MasterGenderCommand command)
            => SaveMasterAsync(
                command,
                _genderService,
                "Gender",
                "MasterAddGender",
                nameof(MasterViewGender));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Gender Status", MenuName = "Master_Gender")]
        public Task<IActionResult> ToggleGender(string uuid)
            => ToggleActiveAsync<MasterGenderDto, MasterGenderCommand>(uuid, _genderService, "Gender");
        #endregion

        #region Honorific
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Honorific List", MenuName = "Master_Honorific")]
        public IActionResult MasterViewHonorific() => View();

        [HttpPost]
        public Task<IActionResult> GetHonorific()
        => GetPagedDataAsync<MasterHonorificDto, MasterHonorificCommand>(_honorificService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleHonorific), "Honorific")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Honorific", MenuName = "Master_Honorific")]
        public IActionResult MasterAddHonorific()
        {
            return View("MasterAddHonorific", new MasterHonorificCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Honorific", MenuName = "Master_Honorific")]
        public Task<IActionResult> MasterEditHonorific(string? uuid)
            => EditMasterAsync(
                uuid,
                _honorificService,
                () => new MasterHonorificCommand { IsActive = true },
                dto => new MasterHonorificCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "MasterAddHonorific",
                nameof(MasterViewHonorific));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Honorific", MenuName = "Master_Honorific")]
        public Task<IActionResult> MasterAddHonorific(MasterHonorificCommand command)
            => SaveMasterAsync(
                command,
                _honorificService,
                "Honorific",
                "MasterAddHonorific",
                nameof(MasterViewHonorific));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Honorific Status", MenuName = "Master_Honorific")]
        public Task<IActionResult> ToggleHonorific(string uuid)
            => ToggleActiveAsync<MasterHonorificDto, MasterHonorificCommand>(uuid, _honorificService, "Honorific");

        #endregion

        #region Email Credential      
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Email Credential List", MenuName = "Master_EmailCredential")]
        public IActionResult MasterViewEmailCredential() => View();

        [HttpPost]
        public Task<IActionResult> GetEmailCredential()
       => GetPagedDataAsync<MasterEmailCredentialDto, MasterEmailCredentialCommand>(_emailCredentialService, dto => new Dictionary<string, object>
       {
           ["uuid"] = dto.UUID,
           ["name"] = dto.EmailAddress,
           ["hostserviceprovider"] = dto.HostServiceProvider,
           ["smtp"] = dto.SMTP,
           ["port"] = dto.Port,
           ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleEmailCredential), "Email Credential")
       });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Email Credential", MenuName = "Master_EmailCredential")]
        public IActionResult MasterAddEmailCredential()
        {
            return View("MasterAddEmailCredential", new MasterEmailCredentialCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Email Credential", MenuName = "Master_EmailCredential")]
        public Task<IActionResult> MasterEditEmailCredential(string? uuid)
            => EditMasterAsync(
                uuid,
                _emailCredentialService,
                () => new MasterEmailCredentialCommand { IsActive = true },
                dto => new MasterEmailCredentialCommand
                {
                    UUID = dto.UUID,
                    EmailAddress = dto.EmailAddress,
                    Password = string.Empty,
                    HostServiceProvider = dto.HostServiceProvider,
                    SMTP = dto.SMTP,
                    Port = dto.Port,
                    IsActive = dto.IsActive
                },
                "MasterAddEmailCredential",
                nameof(MasterViewEmailCredential));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Email Credential", MenuName = "Master_EmailCredential")]
        public Task<IActionResult> MasterAddEmailCredential(MasterEmailCredentialCommand command)
            => SaveMasterAsync(
                command,
                _emailCredentialService,
                "Email Credential",
                "MasterAddEmailCredential",
                nameof(MasterViewEmailCredential));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Email Credential Status", MenuName = "Master_EmailCredential")]
        public Task<IActionResult> ToggleEmailCredential(string uuid)
            => ToggleActiveAsync<MasterEmailCredentialDto, MasterEmailCredentialCommand>(uuid, _emailCredentialService, "Email Credential");
        #endregion

        #region Email Template
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Email Template List", MenuName = "Master_EmailTemplate")]
        public IActionResult MasterViewEmailTemplate() => View();

        [HttpPost]
        public Task<IActionResult> GetEmailTemplate()
        => GetPagedDataAsync<MasterEmailTemplateDto, MasterEmailTemplateCommand>(_emailTemplateService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["templatename"] = dto.EmailTemplateName,
            ["subject"] = dto.EmailSubject,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleEmailTemplate), "email Template")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Email Template", MenuName = "Master_EmailTemplate")]
        public async Task<IActionResult> MasterAddEmailTemplate()
        {
            // Load email credentials for dropdown
            var emailCredentials = await _emailCredentialService.GetDropdownAsync(x => x.EmailAddress);
            var selectItems = emailCredentials.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            var vm = new MasterEmailTemplateVM
            {
                EmailCredentialList = selectItems
            };

            vm.EmailTemplate = new MasterEmailTemplateCommand { IsActive = true };
            return View("MasterAddEmailTemplate", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Email Template", MenuName = "Master_EmailTemplate")]
        public async Task<IActionResult> MasterEditEmailTemplate(string? uuid)
        {
            // Load email credentials for dropdown
            var emailCredentials = await _emailCredentialService.GetDropdownAsync(x => x.EmailAddress);
            var selectItems = emailCredentials.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            var vm = new MasterEmailTemplateVM
            {
                EmailCredentialList = selectItems
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.EmailTemplate = new MasterEmailTemplateCommand { IsActive = true };
                return View("MasterAddEmailTemplate", vm);
            }

            var dto = await _emailTemplateService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewEmailTemplate));
            }

            vm.EmailTemplate = new MasterEmailTemplateCommand
            {
                UUID = dto.UUID,
                EmailCredentialUUID = dto.EmailCredentialUUID,
                EmailSubject = dto.EmailSubject,
                EmailTemplateName = dto.EmailTemplateName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            return View("MasterAddEmailTemplate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Email Template", MenuName = "Master_EmailTemplate")]
        public async Task<IActionResult> MasterAddEmailTemplate(MasterEmailTemplateVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.EmailTemplate, nameof(vm.EmailTemplate)))
            {
                // repopulate dropdown on validation failure
                var emailCredentials = await _emailCredentialService.GetDropdownAsync(x => x.EmailAddress);
                vm.EmailCredentialList = emailCredentials.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title
                }).ToList();

                return View("MasterAddEmailTemplate", vm);
            }

            try
            {
                await _emailTemplateService.SaveAsync(vm.EmailTemplate, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.EmailTemplate.UUID)
                    ? "Email Template added successfully!"
                    : "Email Template updated successfully!");

                return RedirectToAction(nameof(MasterViewEmailTemplate));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                var emailCredentials = await _emailCredentialService.GetDropdownAsync(x => x.EmailAddress);
                vm.EmailCredentialList = emailCredentials.Select(x => new SelectListItem
                {
                    Value = x.UUID,
                    Text = x.Title
                }).ToList();

                return View("MasterAddEmailTemplate", vm);
            }
        }


        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Email Template Status", MenuName = "Master_EmailTemplate")]
        public Task<IActionResult> ToggleEmailTemplate(string uuid)
            => ToggleActiveAsync<MasterEmailTemplateDto, MasterEmailTemplateCommand>(uuid, _emailTemplateService, "Email Template");
        #endregion

        #region Designation
        // Developed by Krishna
        // Date : 19-03-2026

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Designation List", MenuName = "Master_Designation")]
        public IActionResult MasterViewDesignation() => View();

        [HttpPost]
        public Task<IActionResult> GetDesignation()
        => GetPagedDataAsync<MasterDesignationDto, MasterDesignationCommand>(_designationService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleDesignation), "designation")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Designation", MenuName = "Master_Designation")]
        public IActionResult MasterAddDesignation()
        {
            return View("MasterAddDesignation", new MasterDesignationCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Designation", MenuName = "Master_Designation")]
        public Task<IActionResult> MasterEditDesignation(string? uuid)
            => EditMasterAsync(
                uuid,
                _designationService,
                () => new MasterDesignationCommand { IsActive = true },
                dto => new MasterDesignationCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortTitle = dto.ShortTitle,
                    IsActive = dto.IsActive
                },
                "MasterAddDesignation",
                nameof(MasterViewDesignation));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Designation", MenuName = "Master_Designation")]
        public Task<IActionResult> MasterAddDesignation(MasterDesignationCommand command)
            => SaveMasterAsync(
                command,
                _designationService,
                "Designation",
                "MasterAddDesignation",
                nameof(MasterViewDesignation));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Designation Status", MenuName = "Master_Designation")]
        public Task<IActionResult> ToggleDesignation(string uuid)
            => ToggleActiveAsync<MasterDesignationDto, MasterDesignationCommand>(uuid, _designationService, "Designation");
        #endregion

        #region Industry
        // Developed by Krishna (19-03-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Industry List", MenuName = "Master_Industry")]
        public IActionResult MasterViewIndustry() => View();

        [HttpPost]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Get Master Industry List", MenuName = "Master_Industry")]
        public Task<IActionResult> GetIndustry()
          => GetPagedDataAsync<MasterIndustryDto, MasterIndustryCommand>(_industryService, dto => new Dictionary<string, object>
          {
              ["uuid"] = dto.UUID,
              ["title"] = dto.Title ?? "",
              ["image"] = !string.IsNullOrEmpty(dto.Image)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.Image)}' alt='Industry Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "", // ADDED
              ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleIndustry), "industry")
          });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Industry", MenuName = "Master_Industry")]
        public IActionResult MasterAddIndustry()
        {
            return View("MasterAddIndustry", new MasterIndustryCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Industry", MenuName = "Master_Industry")]
        public Task<IActionResult> MasterEditIndustry(string? uuid)
           => EditMasterAsync(
               uuid,
               _industryService,
               () => new MasterIndustryCommand { IsActive = true },
               dto => new MasterIndustryCommand
               {
                   UUID = dto.UUID,
                   Title = dto.Title,
                   Image = dto.Image,
                   Icon = dto.Icon,
                   ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Image),
                   IconUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Icon),
                   Description = dto.Description,
                   IsActive = dto.IsActive
               },
               "MasterAddIndustry",
               nameof(MasterViewIndustry));



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Industry", MenuName = "Master_Industry")]
        public async Task<IActionResult> MasterAddIndustry(MasterIndustryCommand command)
        {
            if (command.Image == null && command.IndustryImageFile == null)
            {
                ModelState.AddModelError("Command.IndustryImageFile", "Required!");
            }
            if (command.Icon == null && command.IndustryIconFile == null)
            {
                ModelState.AddModelError("Command.IndustryIconFile", "Required!");
            }
            if (!ModelState.IsValid)
            {

                return View("MasterAddIndustry", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (command.IndustryImageFile != null)
                {

                    command.Image = await _fileUploadService.SaveFileAsync(
                        command.IndustryImageFile,
                        await GetCompanyNameAsync(),
                        "industry",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.Image);
                }

                if (command.IndustryIconFile != null)
                {
                    command.Icon = await _fileUploadService.SaveFileAsync(
                        command.IndustryIconFile,
                        await GetCompanyNameAsync(),
                        "industry",
                        allowedExtensions
                        );
                    command.IconUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.Icon);
                }

                await _industryService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Industry added successfully!"
                    : "Industry updated successfully!");

                return RedirectToAction(nameof(MasterViewIndustry));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("MasterAddIndustry", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Industry Status", MenuName = "Master_Industry")]
        public Task<IActionResult> ToggleIndustry(string uuid)
            => ToggleActiveAsync<MasterIndustryDto, MasterIndustryCommand>(uuid, _industryService, "Industry");
        #endregion

        #region Department
        // Developed By : Dixita Solanki (19-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Department List", MenuName = "Master_Department")]
        public IActionResult MasterViewDepartment() => View();

        [HttpPost]
        public Task<IActionResult> GetDepartment()
        => GetPagedDataAsync<MasterDepartmentDto, MasterDepartmentCommand>(_departmentService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleDepartment), "department")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Department", MenuName = "Master_Department")]
        public IActionResult MasterAddDepartment()
        {
            return View("MasterAddDepartment", new MasterDepartmentCommand
            {
                IsActive = true
            });
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Department", MenuName = "Master_Department")]
        public Task<IActionResult> MasterEditDepartment(string? uuid)
            => EditMasterAsync(
                uuid,
                _departmentService,
                () => new MasterDepartmentCommand { IsActive = true },
                dto => new MasterDepartmentCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortTitle = dto.ShortTitle,
                    IsActive = dto.IsActive
                },
                "MasterAddDepartment",
                nameof(MasterViewDepartment));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Department", MenuName = "Master_Department")]
        public Task<IActionResult> MasterAddDepartment(MasterDepartmentCommand command)
            => SaveMasterAsync(
                command,
                _departmentService,
                "department",
                "MasterAddDepartment",
                nameof(MasterViewDepartment));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Department Status", MenuName = "Master_Department")]
        public Task<IActionResult> ToggleDepartment(string uuid)
            => ToggleActiveAsync<MasterDepartmentDto, MasterDepartmentCommand>(uuid, _departmentService, "Department");
        #endregion

        #region Permission
        //devlopby dixita

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Permission List", MenuName = "Master_Permission")]
        public IActionResult MasterViewPermission() => View();

        [HttpPost]
        public Task<IActionResult> GetPermission()
        => GetPagedDataAsync<MasterPermissionDto, MasterPermissionCommand>(_permissionService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["description"] = dto.Description ?? "",
            ["permissionGroupTitle"] = dto.PermissionGroupUUID,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(TogglePermission), "permission")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Permission", MenuName = "Master_Permission")]
        public async Task<IActionResult> MasterAddPermission()
        {
            var vm = new MasterPermissionVM
            {
                PermissionGroupList = await LoadDropdownAsync(_PermissionGroupService, x => x.Title)
            };
            vm.Permission = new MasterPermissionCommand { IsActive = true };
            return View("MasterAddPermission", vm);

        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Permission", MenuName = "Master_Permission")]
        public async Task<IActionResult> MasterEditPermission(string? uuid)
        {
            var vm = new MasterPermissionVM
            {
                PermissionGroupList = await LoadDropdownAsync(_PermissionGroupService, x => x.Title)
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Permission = new MasterPermissionCommand { IsActive = true };
                return View("MasterAddPermission", vm);
            }

            var dto = await _permissionService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewPermission));
            }

            vm.Permission = new MasterPermissionCommand
            {
                UUID = dto.UUID,
                Name = dto.Name,
                Description = dto.Description,
                PermissionGroupUUID = dto.PermissionGroupUUID,
                IsActive = dto.IsActive
            };

            return View("MasterAddPermission", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Permission", MenuName = "Master_Permission")]
        public async Task<IActionResult> MasterAddPermission(MasterPermissionVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!TryValidateModel(vm.Permission, nameof(vm.Permission)))
            {
                vm.PermissionGroupList = await LoadDropdownAsync(_PermissionGroupService, x => x.Title);
                return View("MasterAddPermission", vm);
            }

            try
            {
                await _permissionService.SaveAsync(vm.Permission, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Permission.UUID)
                    ? "Permission added successfully!"
                    : "Permission updated successfully!");

                return RedirectToAction(nameof(MasterViewPermission));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.PermissionGroupList = await LoadDropdownAsync(_PermissionGroupService, x => x.Title);
                return View("MasterAddPermission", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Permission Status", MenuName = "Master_Permission")]
        public Task<IActionResult> TogglePermission(string uuid)
            => ToggleActiveAsync<MasterPermissionDto, MasterPermissionCommand>(uuid, _permissionService, "Permission");

        #endregion

        #region WL Permission


        [HttpGet]
        public IActionResult WLMasterViewPermission() => View();

        [HttpPost]
        public Task<IActionResult> WLGetPermission()
        => GetPagedDataAsync<WLMasterPermissionDto, WLMasterPermissionCommand>(_wlMasterPermissionService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["description"] = dto.Description ?? "",
            ["permissionGroupTitle"] = dto.PermissionGroupUUID,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(WLTogglePermission), "permission")
        });

        [HttpGet]
        public async Task<IActionResult> WLMasterAddPermission()
        {
            var vm = new WLMasterPermissionVM
            {
                PermissionGroupList = await LoadDropdownAsync(_wlpermissionGroupService, x => x.Title)
            };
            vm.Permission = new WLMasterPermissionCommand { IsActive = true };
            return View("WLMasterAddPermission", vm);

        }


        [HttpGet]
        public async Task<IActionResult> WLMasterEditPermission(string? uuid)
        {
            var vm = new WLMasterPermissionVM
            {
                PermissionGroupList = await LoadDropdownAsync(_wlpermissionGroupService, x => x.Title)
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Permission = new WLMasterPermissionCommand { IsActive = true };
                return View("WLMasterAddPermission", vm);
            }

            var dto = await _wlMasterPermissionService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(WLMasterViewPermission));
            }

            vm.Permission = new WLMasterPermissionCommand
            {
                UUID = dto.UUID,
                Name = dto.Name,
                Description = dto.Description,
                PermissionGroupUUID = dto.PermissionGroupUUID,
                IsActive = dto.IsActive
            };

            return View("WLMasterAddPermission", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WLMasterAddPermission(WLMasterPermissionVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!TryValidateModel(vm.Permission, nameof(vm.Permission)))
            {
                vm.PermissionGroupList = await LoadDropdownAsync(_wlpermissionGroupService, x => x.Title);
                return View("WLMasterAddPermission", vm);
            }

            try
            {
                await _wlMasterPermissionService.SaveAsync(vm.Permission, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Permission.UUID)
                    ? "Permission added successfully!"
                    : "Permission updated successfully!");

                return RedirectToAction(nameof(WLMasterViewPermission));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.PermissionGroupList = await LoadDropdownAsync(_wlpermissionGroupService, x => x.Title);
                return View("WLMasterAddPermission", vm);
            }
        }

        [HttpPost]
        public Task<IActionResult> WLTogglePermission(string uuid)
            => ToggleActiveAsync<WLMasterPermissionDto, WLMasterPermissionCommand>(uuid, _wlMasterPermissionService, "Permission");

        #endregion

        #region Country
        // Developed By : Krishna (20-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Country List", MenuName = "Master_Country")]
        public IActionResult MasterViewCountry() => View();

        [HttpPost]
        public Task<IActionResult> GetCountry()
        => GetPagedDataAsync<MasterCountryDto, MasterCountryCommand>(_countryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCountry), "country")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Country", MenuName = "Master_Country")]
        public IActionResult MasterAddCountry()
        {
            return View(new MasterCountryCommand { IsActive = true });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Country", MenuName = "Master_Country")]
        public Task<IActionResult> MasterEditCountry(string? uuid)
            => EditMasterAsync(
                uuid,
                _countryService,
                () => new MasterCountryCommand { IsActive = true },
                dto => new MasterCountryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortTitle = dto.ShortTitle,
                    IsActive = dto.IsActive
                },
                "MasterAddCountry",
                nameof(MasterViewCountry));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Country", MenuName = "Master_Country")]
        public Task<IActionResult> MasterAddCountry(MasterCountryCommand command)
            => SaveMasterAsync(
                command,
                _countryService,
                "Country",
                "MasterAddCountry",
                nameof(MasterViewCountry));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Country Status", MenuName = "Master_Country")]
        public Task<IActionResult> ToggleCountry(string uuid)
            => ToggleActiveAsync<MasterCountryDto, MasterCountryCommand>(uuid, _countryService, "Country");
        #endregion

        #region State
        // Developed By : Krishna (20-03-2026)
        private async Task LoadCountryDropdown(MasterStateVM vm)
        {
            var country = await _countryService.GetDropdownAsync(x => x.Title);
            vm.CountryList = country.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();


        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master State List", MenuName = "Master_State")]
        public IActionResult MasterViewState() => View();

        [HttpPost]
        public Task<IActionResult> GetState()
        => GetPagedDataAsync<MasterStateDto, MasterStateCommand>(_stateService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["country"] = dto.CountryUUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleState), "state")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master State", MenuName = "Master_State")]
        public async Task<IActionResult> MasterAddState()
        {
            var vm = new MasterStateVM();
            await LoadCountryDropdown(vm);

            vm.State = new MasterStateCommand { IsActive = true };
            return View("MasterAddState", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master State", MenuName = "Master_State")]
        public async Task<IActionResult> MasterEditState(string? uuid)
        {
            var vm = new MasterStateVM();
            await LoadCountryDropdown(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                vm.State = new MasterStateCommand { IsActive = true };
                return View("MasterAddState", vm);
            }

            var dto = await _stateService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewState));
            }

            vm.State = new MasterStateCommand
            {
                UUID = dto.UUID,
                CountryUUID = dto.CountryUUID,
                Title = dto.Title,
                ShortTitle = dto.ShortTitle,
                IsActive = dto.IsActive
            };

            return View("MasterAddState", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master State", MenuName = "Master_State")]
        public async Task<IActionResult> MasterAddState(MasterStateVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadCountryDropdown(vm);
                return View("MasterAddState", vm);
            }

            try
            {
                SetSuccessMessage(string.IsNullOrEmpty(vm.State.UUID)
                    ? "State added successfully!"
                    : "State updated successfully!");

                await _stateService.SaveAsync(vm.State, GetUserUUID(), Utils.GetLocalIPAddress());

                

                return RedirectToAction(nameof(MasterViewState));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                await LoadCountryDropdown(vm);
                return View("MasterAddState", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master State Status", MenuName = "Master_State")]
        public Task<IActionResult> ToggleState(string uuid)
            => ToggleActiveAsync<MasterStateDto, MasterStateCommand>(uuid, _stateService, "State");
        #endregion

        #region City
        // Developed By : Krishna
        // Date : 28-02-2026
        private async Task LoadCountryCity(MasterCityVM vm)
        {
            var country = await _countryService.GetDropdownAsync(x => x.Title);
            vm.CountryList = country.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var state = await _stateService.GetDropdownAsync(x => x.Title);
            var stateList = state.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master City List", MenuName = "Master_City")]
        public IActionResult MasterViewCity() => View();

        [HttpPost]
        public Task<IActionResult> GetCity()
        => GetPagedDataAsync<MasterCityDto, MasterCityCommand>(_cityService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["country"] = dto.CountryUUID,
            ["state"] = dto.StateUUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCity), "city")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master City", MenuName = "Master_City")]
        public async Task<IActionResult> MasterAddCity()
        {
            // Load email credentials for dropdown
            var country = await _countryService.GetDropdownAsync(x => x.Title);
            var state = await _stateService.GetDropdownAsync(x => x.Title);
            var countryList = country.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var stateList = state.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var vm = new MasterCityVM
            {
                CountryList = countryList,
                StateList = stateList
            };

            vm.City = new MasterCityCommand { IsActive = true };
            return View("MasterAddCity", vm);
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master City", MenuName = "Master_City")]
        public async Task<IActionResult> MasterEditCity(string? uuid)
        {
            // Load email credentials for dropdown
            var vm = new MasterCityVM();
            await LoadCountryCity(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                vm.City = new MasterCityCommand { IsActive = true };
                return View("MasterAddCity", vm);
            }

            var dto = await _cityService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewCity));
            }

            vm.City = new MasterCityCommand
            {
                UUID = dto.UUID,
                CountryUUID = dto.CountryUUID,
                StateUUID = dto.StateUUID,
                Title = dto.Title,
                ShortTitle = dto.ShortTitle,
                IsActive = dto.IsActive
            };

            return View("MasterAddCity", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master City", MenuName = "Master_City")]
        public async Task<IActionResult> MasterAddCity(MasterCityVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!ModelState.IsValid)
            {
                await LoadCountryCity(vm);
                return View("MasterAddCity", vm);
            }

            try
            {
                await _cityService.SaveAsync(vm.City, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.City.UUID)
                    ? "City added successfully!"
                    : "City updated successfully!");

                return RedirectToAction(nameof(MasterViewCity));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                await LoadCountryCity(vm);
                return View("MasterAddCity", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master City Status", MenuName = "Master_City")]
        public Task<IActionResult> ToggleCity(string uuid)
            => ToggleActiveAsync<MasterCityDto, MasterCityCommand>(uuid, _cityService, "City");



        #endregion

        #region Nomenclature

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Nomenclature List", MenuName = "Master_Nomenclature")]
        public IActionResult MasterViewNomenclature() => View();

        [HttpPost]
        public Task<IActionResult> GetNomenclature()
        => GetPagedDataAsync<MasterNomenclatureDto, MasterNomenclatureCommand>(_nomenclatureService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["key"] = dto.ModuleKey,
            ["prefix"] = dto.Prefix,
            ["numberofdigit"] = dto.NumberOfDigits,
            ["financialyear"] = dto.FinancialYearUUID ?? " - ",
            ["startno"] = dto.StartNo,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleNomenclature), "nomenclature")
        });

        private async Task LoadNomenclatureDropdowns(MasterNomenclatureCommand model)
        {
            var financialYears = await _financialYearService.GetDropdownAsync(x => x.Title);

            model.YearList = financialYears.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Nomenclature", MenuName = "Master_Nomenclature")]
        public async Task<IActionResult> MasterEditNomenclature(string? uuid)
        {
            var model = new MasterNomenclatureCommand();

            await LoadNomenclatureDropdowns(model);

            if (string.IsNullOrEmpty(uuid))
            {
                model.IsActive = true;
                return View("MasterAddNomenclature", model);
            }

            var dto = await _nomenclatureService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewNomenclature));
            }
            model.UUID = dto.UUID;
            model.ModuleKey = dto.ModuleKey;
            model.FinancialYearUUID = dto.FinancialYearUUID;
            model.Prefix = dto.Prefix;
            model.NumberOfDigits = dto.NumberOfDigits;
            model.IsIncludeYear = dto.IsIncludeYear;
            model.StartNo = dto.StartNo;
            model.IsActive = dto.IsActive;

            return View("MasterAddNomenclature", model);
        }

        [HttpGet]
        public async Task<IActionResult> MasterAddNomenclature()
        {
            var model = new MasterNomenclatureCommand();

            await LoadNomenclatureDropdowns(model);
            model.IsActive = true;
            return View("MasterAddNomenclature", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Nomenclature", MenuName = "Master_Nomenclature")]
        public async Task<IActionResult> MasterAddNomenclature(MasterNomenclatureCommand command)
        {
            try
            {
                if (command == null)
                    return BadRequest();

                if (!ModelState.IsValid)
                {
                    await LoadNomenclatureDropdowns(command);
                    return View("MasterAddNomenclature", command);
                }
                if (!string.IsNullOrEmpty(command.UUID))
                {
                    var existing = await _nomenclatureService.GetByUuidAsync(command.UUID);
                    if (existing == null)
                    {
                        SetErrorMessage("Record not found!");
                        return RedirectToAction(nameof(MasterViewNomenclature));
                    }

                    command.ModuleKey = existing.ModuleKey;
                }

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                  ? "Nomenclature added successfully!"
                  : "Nomenclature updated successfully!");

                await _nomenclatureService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

               

                return RedirectToAction(nameof(MasterViewNomenclature));
            }
            catch(Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadNomenclatureDropdowns(command);
                return View("MasterAddNomenclature", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Nomenclature Status", MenuName = "Master_Nomenclature")]
        public Task<IActionResult> ToggleNomenclature(string uuid)
            => ToggleActiveAsync<MasterNomenclatureDto, MasterNomenclatureCommand>(uuid, _nomenclatureService, "NomenClature");

        #endregion

        #region Employee
        // Developed By : Dixita Solanki (21-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Employee List", MenuName = "Master_Employee")]
        public IActionResult MasterViewEmployee() => View();
        [HttpPost]
        public async Task<IActionResult> GetEmployee()
        {
            return await GetPagedDataAsync<MasterEmployeeDto, MasterEmployeeCommand>(
                _employeeService,  // service instance
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["employeeid"] = dto.EmployeeCode,
                    ["fullname"] = (string.IsNullOrEmpty(dto.HonorificUUID) ? "" : dto.HonorificUUID + " ")
                                   + dto.FirstName + " " + dto.LastName,
                    ["role"] = dto.RoleUUID,
                    ["department"] = dto.DepartmentUUID,
                    ["designation"] = dto.DesignationUUID,
                    ["loginallowed"] = dto.IsLoginAllowed
                ? "<span class='badge badge-outline-success'>Yes</span>"
                : "<span class='badge badge-outline-danger'>No</span>",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleEmployee), "employee")
                });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Employee", MenuName = "Master_Employee")]
        public async Task<IActionResult> MasterAddEmployee()
        {
            var vm = new MasterEmployeeVM();
            var policy = await _passwordPolicyService.GetFirstAsync();

            // Load all dropdowns
            await LoadDropdowns(vm);
            await SetPasswordPolicyDescription(vm);
            vm.Employee = new MasterEmployeeCommand { IsActive = true };
            return View("MasterAddEmployee", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Employee", MenuName = "Master_Employee")]
        public async Task<IActionResult> MasterEditEmployee(string? uuid)
        {
            var vm = new MasterEmployeeVM();

            // Load all dropdowns
            await LoadDropdowns(vm);
            await SetPasswordPolicyDescription(vm);
            if (string.IsNullOrEmpty(uuid))
            {
                // Add new employee
                vm.Employee = new MasterEmployeeCommand { IsActive = true };
                return View("MasterAddEmployee", vm);
            }
            // Load existing employee
            var dto = await _employeeService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewEmployee));
            }

            vm.Employee = new MasterEmployeeCommand
            {
                UUID = dto.UUID,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Password = dto.Password,
                MobileNumber = dto.MobileNumber,
                EmailId = dto.EmailId,
                BirthDate = dto.BirthDate,
                CompanyEmailID = dto.CompanyEmailID,
                CompanyMobileNumber = dto.CompanyMobileNumber,
                IsLoginAllowed = dto.IsLoginAllowed,
                IsActive = dto.IsActive,
                UserName = dto.UserName,
                GenderUUID = dto.GenderUUID,
                HonorificUUID = dto.HonorificUUID,
                DepartmentUUID = dto.DepartmentUUID,
                RoleUUID = dto.RoleUUID,
                EmployeeCode = dto.EmployeeCode,
                DesignationUUID = dto.DesignationUUID,
                Profile_URL = dto.Profile_URL,
                ProfileUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Profile_URL)
            };

            return View("MasterAddEmployee", vm);
        }


        // BUG [05-04-2026][Owner:Dixita] currently employee code,username can be changed from edit screen using inspect mode which should not be allowed, it should be generated only once when employee is created and then should be read only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Employee", MenuName = "Master_Employee")]
        public async Task<IActionResult> MasterAddEmployee(MasterEmployeeVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdowns(vm);
                    await SetPasswordPolicyDescription(vm);
                    return View("MasterAddEmployee", vm);
                }
                if (string.IsNullOrEmpty(vm.Employee.UUID))
                {
                    vm.Employee.EmployeeCode = await _nomenclatureService.GenerateCodeAsync("Employee");
                }
                else
                {
                    // Edit mode → DO NOT trust incoming value
                    var existingEmployee = await _employeeService.GetByUuidAsync(vm.Employee.UUID);
                    if (existingEmployee == null)
                    {
                        ModelState.AddModelError("", "Employee not found.");
                        await LoadDropdowns(vm);
                        await SetPasswordPolicyDescription(vm);
                        return View("MasterAddEmployee", vm);
                    }

                    vm.Employee.EmployeeCode = existingEmployee.EmployeeCode;
                    vm.Employee.UserName = existingEmployee.UserName;// 🔒 enforce original
                }
                if (string.IsNullOrEmpty(vm.Employee.EmployeeCode))
                {
                    ModelState.AddModelError("", "Employee nomenclature is not configured. Please configure it first.");
                    await LoadDropdowns(vm);
                    await SetPasswordPolicyDescription(vm);
                    return View("MasterAddEmployee", vm);
                }
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                // Handle Profile Upload
                if (vm.Employee.Profile != null)
                {
                    vm.Employee.Profile_URL = await _fileUploadService.SaveFileAsync(
                        vm.Employee.Profile,
                        await GetCompanyNameAsync(),
                        "Employee",
                        allowedExtensions
                        );
                    vm.Employee.ProfileUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.Employee.Profile_URL);

                }
                if (string.IsNullOrEmpty(vm.Employee.UUID) &&
            string.IsNullOrEmpty(vm.Employee.Profile_URL))
                {
                    ModelState.AddModelError("Employee.Profile", "Profile image is required.");
                    await LoadDropdowns(vm);
                    await SetPasswordPolicyDescription(vm);
                    return View("MasterAddEmployee", vm);
                }

                if (!string.IsNullOrEmpty(vm.Employee.Password))
                {
                    var policy = await _passwordPolicyService.GetFirstAsync();
                    var validationError = ValidatePassword(vm.Employee.Password, policy);

                    if (validationError != null)
                    {
                        ModelState.AddModelError("Employee.Password", validationError);
                        await LoadDropdowns(vm);
                        await SetPasswordPolicyDescription(vm);
                        return View("MasterAddEmployee", vm);
                    }
                    vm.Employee.Password = _passwordHasher.HashPassword(vm.Employee, vm.Employee.Password);
                }
                else if (!string.IsNullOrEmpty(vm.Employee.UUID))
                {
                    // Edit mode & password not provided → keep the existing password
                    var existingEmployee = await _employeeService.GetByUuidAsync(vm.Employee.UUID);
                    if (existingEmployee != null)
                    {
                        vm.Employee.Password = existingEmployee.Password;
                    }
                }
                await _employeeService.SaveAsync(
                    vm.Employee,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Employee.UUID)
                    ? "Employee added successfully!"
                    : "Employee updated successfully!");

                return RedirectToAction(nameof(MasterViewEmployee));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadDropdowns(vm);
                await SetPasswordPolicyDescription(vm);
                return View("MasterAddEmployee", vm);
            }
        }
        private async Task LoadDropdowns(MasterEmployeeVM vm)
        {
            vm.Department = await LoadDropdownAsync(_departmentService, x => x.Title);
            vm.Role = await LoadDropdownAsync(_roleService, x => x.Title);
            vm.Gender = await LoadDropdownAsync(_genderService, x => x.Title);
            vm.Honorific = await LoadDropdownAsync(_honorificService, x => x.Title);
            vm.Designation = await LoadDropdownAsync(_designationService, x => x.Title);
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Change Password", MenuName = "Master_Employee")]
        public async Task<IActionResult> ChangePassword(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
                return NotFound();

            var employee = await _employeeService.GetByUuidAsync(uuid);
            if (employee == null)
                return NotFound();
            var policy = await _passwordPolicyService.GetFirstAsync();
            var vm = new ChangePasswordVM
            {
                UUID = uuid,
                PasswordPolicy = policy,
                PasswordPolicyDescription = BuildPasswordPolicyDescription(policy)
            };
            return PartialView("_ChangePassword", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Save New Password", MenuName = "Master_Employee")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            var policy = await _passwordPolicyService.GetFirstAsync();
            vm.PasswordPolicy = policy;

            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Please fill all required fields"
                });
            }
            var validationError = ValidatePassword(vm.NewPassword, policy);
            if (validationError != null)
            {
                return Json(new
                {
                    success = false,
                    field = "NewPassword",
                    message = validationError
                });
            }

            try
            {
                var employee = await _employeeService.GetByUuidAsync(vm.UUID);
                if (employee == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "User not found"
                    });
                }
                var userCommand = new MasterEmployeeCommand
                {
                    Password = employee.Password
                };

                var passwordValid = _passwordHasher.VerifyHashedPassword(
                    userCommand,
                    employee.Password,
                    vm.CurrentPassword
                );

                if (passwordValid == PasswordVerificationResult.Failed)
                {
                    return Json(new
                    {
                        success = false,
                        field = "CurrentPassword",
                        message = "Current password is incorrect"
                    });
                }

                HttpContext.Items["PasswordChangeReason"] = vm.Reason?.Trim();

                await _employeeService.ChangePasswordAsync(vm.UUID, vm.CurrentPassword, vm.NewPassword);

                return Json(new
                {
                    success = true,
                    message = "Password changed successfully!"
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
            finally
            {
                HttpContext.Items.Remove("PasswordChangeReason");
            }
        }
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Employee Status", MenuName = "Master_Employee")]
        public Task<IActionResult> ToggleEmployee(string uuid)
           => ToggleActiveAsync<MasterEmployeeDto, MasterEmployeeCommand>(uuid, _employeeService, "Employee");

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
        private async Task SetPasswordPolicyDescription(MasterEmployeeVM vm)
        {
            var policy = await _passwordPolicyService.GetFirstAsync();
            vm.PasswordPolicyDescription = BuildPasswordPolicyDescription(policy);
        }
        #endregion

        #region Manage Path

        // Developed by Krishna  (20-03-2026)

        private async Task BindPathAndPermissionDropdownsAsync(MasterManagePathVM vm)
        {

            var pathDropdown = await _pathService.GetDropdownAsync(x => x.Path);
            var permissionDropdown = await _permissionService.GetDropdownAsync(x => x.Name);

            vm.PathList = pathDropdown
                .Select(x => new SelectListItem { Value = x.UUID, Text = x.Title })
                .OrderBy(i => i.Text)
                .ToList();

            vm.PermissionList = permissionDropdown
                .Select(x => new SelectListItem { Value = x.UUID, Text = x.Title })
                .OrderBy(i => i.Text)
                .ToList();
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Manage Path List", MenuName = "Master_Manage_Path")]
        public IActionResult MasterViewManagePath() => View();


        [HttpPost]
        public Task<IActionResult> GetManagePath()
        => GetPagedDataAsync<MasterPathPermissionDto, MasterPathPermissionCommand>(_pathpermissionService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["pathname"] = dto.PathUUID,
            ["permissionname"] = dto.PermissionUUID,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleManagePath), "pathpermission")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Manage Path", MenuName = "Master_Manage_Path")]
        public async Task<IActionResult> MasterAddManagePath()
        {
            var vm = new MasterManagePathVM();
            await BindPathAndPermissionDropdownsAsync(vm);
            vm.ManagePath = new MasterPathPermissionCommand { IsActive = true };
            return View("MasterAddManagePath", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Manage Path", MenuName = "Master_Manage_Path")]
        public async Task<IActionResult> MasterEditManagePath(string? uuid)
        {

            var vm = new MasterManagePathVM();
            await BindPathAndPermissionDropdownsAsync(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                vm.ManagePath = new MasterPathPermissionCommand { IsActive = true };
                return View("MasterAddManagePath", vm);
            }

            var dto = await _pathpermissionService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewManagePath));
            }

            vm.ManagePath = new MasterPathPermissionCommand
            {
                UUID = dto.UUID,
                PathUUID = dto.PathUUID,
                PermissionUUID = dto.PermissionUUID,
                IsActive = dto.IsActive
            };

            return View("MasterAddManagePath", vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Manage Path", MenuName = "Master_Manage_Path")]
        public async Task<IActionResult> MasterAddManagePath(MasterManagePathVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.ManagePath, nameof(vm.ManagePath)))
            {
                // repopulate dropdown on validation failure
                await BindPathAndPermissionDropdownsAsync(vm);
                return View("MasterAddManagePath", vm);
            }

            try
            {
                await _pathpermissionService.SaveAsync(vm.ManagePath, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.ManagePath.UUID)
                    ? "Manage Path added successfully!"
                    : "Manage Path added successfully!");

                return RedirectToAction(nameof(MasterViewManagePath));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                // Reload dropdown on error
                await BindPathAndPermissionDropdownsAsync(vm);

                return View("MasterAddManagePath", vm);
            }
        }




        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Manage Path Status", MenuName = "Master_Manage_Path")]
        public Task<IActionResult> ToggleManagePath(string uuid)
            => ToggleActiveAsync<MasterPathPermissionDto, MasterPathPermissionCommand>(uuid, _pathpermissionService, "Manage Path");

        #endregion

        #region Role 
        // Developed by Dixita (20-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Role List", MenuName = "Master_Role")]
        public IActionResult MasterViewRole() => View();

        [HttpPost]
        public Task<IActionResult> GetRoles()
        => GetPagedDataAsync<MasterRoleDto, MasterRoleCommand>(_roleService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["shortname"] = dto.ShortTitle,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleRole), "Role")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Role", MenuName = "Master_Role")]
        public IActionResult MasterAddRole()
        {
            return View("MasterAddRole", new MasterRoleCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Role", MenuName = "Master_Role")]
        public Task<IActionResult> MasterEditRole(string? uuid)
            => EditMasterAsync(
                uuid,
                _roleService,
                () => new MasterRoleCommand { IsActive = true },
                dto => new MasterRoleCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    ShortTitle = dto.ShortTitle,
                    IsActive = dto.IsActive
                },
                "MasterAddRole",
                nameof(MasterViewRole));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Role", MenuName = "Master_Role")]
        public Task<IActionResult> MasterAddRole(MasterRoleCommand command)
            => SaveMasterAsync(
                command,
                _roleService,
                "Role",
                "MasterAddRole",
                nameof(MasterViewRole));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Role Status", MenuName = "Master_Role")]
        public Task<IActionResult> ToggleRole(string uuid)
            => ToggleActiveAsync<MasterRoleDto, MasterRoleCommand>(uuid, _roleService, "Role");
        #endregion

        #region Master Menu
        // Developed by Dixita (20-03-2026)
        private List<SelectListItem> BuildMenuLevels()
        {
            return new List<SelectListItem>
           {
            new SelectListItem { Text = "First Level", Value = "1" },
            new SelectListItem { Text = "Second Level", Value = "2" },
            new SelectListItem { Text = "Third Level", Value = "3" }
           };
        }

        [HttpGet]
        public async Task<IActionResult> GetSubParents(string mainUUID)
        {
            var subParents = await _menuService.GetSubParentsAsync(mainUUID);
            var result = subParents.Select(s => new { value = s.UUID, text = s.MenuName }).ToList();
            return Json(result);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Menu List", MenuName = "Master_Menu")]
        public IActionResult MasterViewMenu() => View();

        [HttpPost]
        public Task<IActionResult> GetMenu()
        => GetPagedDataAsync<MasterMenuDto, MasterMenuCommand>(_menuService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.MenuName,
            ["level"] = dto.MenuLevel == 1 ? "First Level" : dto.MenuLevel == 2 ? "Second Level" : "Third Level",
            ["permission"] = dto.PermissionUUID,
            ["url"] = dto.Url,
            ["isparent"] = dto.IsParent ? "<span class=\"badge badge-outline-success\">Yes</span>" : "<span class=\"badge badge-outline-danger\">No</span>",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleMenuStatus), "menu")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Menu", MenuName = "Master_Menu")]
        public async Task<IActionResult> MasterAddMenu()
        {
            var mainParents = await _menuService.GetMainParentsAsync();
            var permission = await _permissionService.GetDropdownAsync(x => x.Name);
            var masterMenu = new MasterMenuVm
            {
                MenuLevelList = BuildMenuLevels(),
                ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList(),
                SubMenuList = new List<SelectListItem>(),
                PermissionList = permission.Select(p => new SelectListItem { Value = p.UUID, Text = p.Title }).ToList()

            };
            masterMenu.MasterMenu = new MasterMenuCommand { IsActive = true };
            return View("MasterAddMenu", masterMenu);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Menu", MenuName = "Master_Menu")]
        public async Task<IActionResult> MasterEditMenu(string? uuid)
        {
            var mainParents = await _menuService.GetMainParentsAsync();
            var permission = await _permissionService.GetDropdownAsync(x => x.Name);
            var masterMenu = new MasterMenuVm
            {
                MenuLevelList = BuildMenuLevels(),
                ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList(),
                SubMenuList = new List<SelectListItem>(),
                PermissionList = permission.Select(p => new SelectListItem { Value = p.UUID, Text = p.Title }).ToList()

            };

            if (string.IsNullOrEmpty(uuid))
            {
                // Add new menu
                masterMenu.MasterMenu = new MasterMenuCommand { IsActive = true };
                return View("MasterAddMenu", masterMenu);
            }

            // Edit menu
            var menuDetails = await _menuService.GetByUuidAsync(uuid);
            if (menuDetails == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewMenu));
            }
            string? mainParentUUID = menuDetails.MainParentUUID;
            string? subParentUUID = null;

            // 🔥 IMPORTANT: detect if saved value is actually a subparent
            if (!string.IsNullOrEmpty(mainParentUUID))
            {
                // get all main parents' subparents
                var parent = await _menuService.GetByUuidAsync(mainParentUUID);
                if (parent != null && parent.MenuLevel == 2)
                {
                    subParentUUID = parent.UUID;
                    mainParentUUID = parent.MainParentUUID;
                }
            }
            masterMenu.MasterMenu = new MasterMenuCommand
            {
                UUID = menuDetails.UUID,
                MenuName = menuDetails.MenuName,
                MenuLevel = menuDetails.MenuLevel,
                MainParentUUID = mainParentUUID,
                SubParentUUID = subParentUUID,
                Sequence = menuDetails.Sequence,
                Url = menuDetails.Url,
                IsParent = menuDetails.IsParent,
                IsActive = menuDetails.IsActive,
                PermissionUUID = menuDetails.PermissionUUID,
                MenuIcon = menuDetails.MenuIcon
            };

            // Load sub-parents if MainParentUUID exists
            if (!string.IsNullOrEmpty(mainParentUUID))
            {
                var subParents = await _menuService.GetSubParentsAsync(mainParentUUID);
                masterMenu.SubMenuList = subParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName, Selected = s.UUID == subParentUUID }).ToList();
            }

            return View("MasterAddMenu", masterMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Menu", MenuName = "Master_Menu")]
        public async Task<IActionResult> MasterAddMenu(MasterMenuVm model)
        {
            var mainParents = await _menuService.GetMainParentsAsync();
            var subParents = !string.IsNullOrEmpty(model.MasterMenu.MainParentUUID)
                ? await _menuService.GetSubParentsAsync(model.MasterMenu.MainParentUUID)
                : new List<MasterMenuDto>();
            var permission = await _permissionService.GetDropdownAsync(x => x.Name);

            model.MenuLevelList = BuildMenuLevels();
            model.ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList();
            model.SubMenuList = subParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList();
            var selectPermission = permission.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            if (model?.MasterMenu?.IsParent == true)
            {
                model.MasterMenu.Url = null;
                model.MasterMenu.PermissionUUID = null;
                ModelState.Remove("MasterMenu.Url");
                ModelState.Remove("MasterMenu.PermissionUUID");
            }

            if (!ModelState.IsValid)
            {
                return View("MasterAddMenu", model);
            }
            try
            {
                if (!string.IsNullOrEmpty(model.MasterMenu.SubParentUUID))
                {
                    model.MasterMenu.MainParentUUID = model.MasterMenu.SubParentUUID;
                    model.MasterMenu.SubParentUUID = null;
                }

                await _menuService.SaveAsync(model.MasterMenu, GetUserUUID(), Utils.GetLocalIPAddress());
                _menuQueryService?.InvalidateCache();

                SetSuccessMessage(string.IsNullOrEmpty(model.MasterMenu.UUID)
                    ? "Menu added successfully!"
                    : "Menu updated successfully!");

                return RedirectToAction(nameof(MasterViewMenu));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("MasterAddMenu", model);
            }
        }

        //[HttpPost]
        //public Task<IActionResult> ToggleMenuStatus(string uuid)
        //    => ToggleActiveAsync<MasterMenuDto, MasterMenuCommand>(uuid, _menuService, "Menu");


        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Menu Status", MenuName = "Master_Menu")]
        public async Task<IActionResult> ToggleMenuStatus(string uuid)
        {
            try
            {
                var newStatus = await _menuService.ToggleActiveAsync(uuid, GetUserUUID(), Utils.GetLocalIPAddress());
                _menuQueryService?.InvalidateCache();
                return Json(new
                {
                    success = true,
                    isActive = newStatus,
                    message = "Menu status updated successfully!"
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


        #endregion

        #region WL Master Menu

        private List<SelectListItem> WLBuildMenuLevels()
        {
            return new List<SelectListItem>
           {
            new SelectListItem { Text = "First Level", Value = "1" },
            new SelectListItem { Text = "Second Level", Value = "2" },
            new SelectListItem { Text = "Third Level", Value = "3" }
           };
        }

        [HttpGet]
        public async Task<IActionResult> WLGetSubParents(string mainUUID)
        {
            var subParents = await _wlMasterMenuService.GetSubParentsAsync(mainUUID);
            var result = subParents.Select(s => new { value = s.UUID, text = s.MenuName }).ToList();
            return Json(result);
        }

        [HttpGet]
        public IActionResult WLMasterViewMenu() => View();

        [HttpPost]
        public Task<IActionResult> WLGetMenu()
        => GetPagedDataAsync<WLMasterMenuDto, WLMasterMenuCommand>(_wlMasterMenuService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.MenuName,
            ["level"] = dto.MenuLevel == 1 ? "First Level" : dto.MenuLevel == 2 ? "Second Level" : "Third Level",
            ["url"] = dto.Url,
            ["isparent"] = dto.IsParent ? "<span class=\"badge badge-outline-success\">Yes</span>" : "<span class=\"badge badge-outline-danger\">No</span>",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(WLToggleMenuStatus), "menu")
        });

        [HttpGet]
        public async Task<IActionResult> WLMasterAddMenu()
        {
            var mainParents = await _wlMasterMenuService.GetMainParentsAsync();
            var permission = await _wlMasterPermissionService.GetDropdownAsync(x => x.Name);
            var masterMenu = new WLMasterMenuVM
            {
                MenuLevelList = WLBuildMenuLevels(),
                ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList(),
                SubMenuList = new List<SelectListItem>(),
                PermissionList = permission.Select(p => new SelectListItem { Value = p.UUID, Text = p.Title }).ToList()

            };
            masterMenu.MasterMenu = new WLMasterMenuCommand { IsActive = true };
            return View("WLMasterAddMenu", masterMenu);
        }

        [HttpGet]
        public async Task<IActionResult> WLMasterEditMenu(string? uuid)
        {
            var mainParents = await _wlMasterMenuService.GetMainParentsAsync();
            var permission = await _wlMasterPermissionService.GetDropdownAsync(x => x.Name);
            var masterMenu = new WLMasterMenuVM
            {
                MenuLevelList = WLBuildMenuLevels(),
                ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList(),
                SubMenuList = new List<SelectListItem>(),
                PermissionList = permission.Select(p => new SelectListItem { Value = p.UUID, Text = p.Title }).ToList()

            };

            if (string.IsNullOrEmpty(uuid))
            {
                // Add new menu
                masterMenu.MasterMenu = new WLMasterMenuCommand { IsActive = true };
                return View("WLMasterAddMenu", masterMenu);
            }

            // Edit menu
            var menuDetails = await _wlMasterMenuService.GetByUuidAsync(uuid);
            if (menuDetails == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewMenu));
            }

            masterMenu.MasterMenu = new WLMasterMenuCommand
            {
                UUID = menuDetails.UUID,
                MenuName = menuDetails.MenuName,
                MenuLevel = menuDetails.MenuLevel,
                MainParentUUID = menuDetails.MainParentUUID,
                SubParentUUID = menuDetails.SubParentUUID,
                Sequence = menuDetails.Sequence,
                Url = menuDetails.Url,
                IsParent = menuDetails.IsParent,
                IsActive = menuDetails.IsActive,
                PermissionUUID = menuDetails.PermissionUUID,
                MenuIcon = menuDetails.MenuIcon
            };

            if (menuDetails.MenuLevel == 3)
            {
                // For Level 3 menus: MainParentUUID stores the SubParent UUID
                // Fetch that SubParent record to get its parent (MainParent) UUID
                if (!string.IsNullOrEmpty(menuDetails.MainParentUUID))
                {
                    var subParentRecord = await _wlMasterMenuService.GetByUuidAsync(menuDetails.MainParentUUID);
                    if (subParentRecord != null && !string.IsNullOrEmpty(subParentRecord.MainParentUUID))
                    {
                        // Now fetch all sub-parents of the actual main parent
                        var actualMainParentUUID = subParentRecord.MainParentUUID;
                        var subParents = await _wlMasterMenuService.GetSubParentsAsync(actualMainParentUUID);

                        // Populate SubMenuList with proper selection
                        masterMenu.SubMenuList = subParents.Select(s => new SelectListItem
                        {
                            Value = s.UUID,
                            Text = s.MenuName,
                            Selected = s.UUID == menuDetails.MainParentUUID
                        }).ToList();

                        // Mark the MainParent as selected in ParentMenuList
                        masterMenu.ParentMenuList = mainParents.Select(s => new SelectListItem
                        {
                            Value = s.UUID,
                            Text = s.MenuName,
                            Selected = s.UUID == actualMainParentUUID
                        }).ToList();

                        // Update the model to reflect the actual main parent
                        masterMenu.MasterMenu.MainParentUUID = actualMainParentUUID;
                        masterMenu.MasterMenu.SubParentUUID = menuDetails.MainParentUUID;
                    }
                }
            }


            return View("WLMasterAddMenu", masterMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WLMasterAddMenu(WLMasterMenuVM model)
        {
            var mainParents = await _wlMasterMenuService.GetMainParentsAsync();
            var subParents = !string.IsNullOrEmpty(model.MasterMenu.MainParentUUID)
                ? await _wlMasterMenuService.GetSubParentsAsync(model.MasterMenu.MainParentUUID)
                : new List<WLMasterMenuDto>();
            var permission = await _wlMasterPermissionService.GetDropdownAsync(x => x.Name);

            model.MenuLevelList = WLBuildMenuLevels();
            model.ParentMenuList = mainParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList();
            model.SubMenuList = subParents.Select(s => new SelectListItem { Value = s.UUID, Text = s.MenuName }).ToList();
            var selectPermission = permission.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title,
            }).ToList();

            if (model?.MasterMenu?.IsParent == true)
            {
                model.MasterMenu.Url = null;
                model.MasterMenu.PermissionUUID = null;
                ModelState.Remove("MasterMenu.Url");
                ModelState.Remove("MasterMenu.PermissionUUID");
            }

            if (!ModelState.IsValid)
            {
                return View("WLMasterAddMenu", model);
            }
            try
            {
                if (model.MasterMenu.MenuLevel == 3)
                {
                    model.MasterMenu.MainParentUUID = null;
                    model.MasterMenu.MainParentUUID = model.MasterMenu.SubParentUUID;
                    model.MasterMenu.SubParentUUID = null;
                }


                await _wlMasterMenuService.SaveAsync(model.MasterMenu, GetUserUUID(), Utils.GetLocalIPAddress());
                _menuQueryService?.InvalidateCache();

                SetSuccessMessage(string.IsNullOrEmpty(model.MasterMenu.UUID)
                    ? "Menu added successfully!"
                    : "Menu updated successfully!");

                return RedirectToAction(nameof(WLMasterViewMenu));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("WLMasterAddMenu", model);
            }
        }

        //[HttpPost]
        //public Task<IActionResult> ToggleMenuStatus(string uuid)
        //    => ToggleActiveAsync<MasterMenuDto, MasterMenuCommand>(uuid, _menuService, "Menu");


        [HttpPost]
        public async Task<IActionResult> WLToggleMenuStatus(string uuid)
        {
            try
            {
                var newStatus = await _wlMasterMenuService.ToggleActiveAsync(uuid, GetUserUUID(), Utils.GetLocalIPAddress());
                _menuQueryService?.InvalidateCache();
                return Json(new
                {
                    success = true,
                    isActive = newStatus,
                    message = "Menu status updated successfully!"
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


        #endregion

        #region Testimonial
        //develop by Dixita
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Testimonial List", MenuName = "Master_Testimonial")]
        public IActionResult MasterViewTestimonial() => View();

        [HttpPost]
        public Task<IActionResult> GetTestimonials()
        => GetPagedDataAsync<MasterTestimonialDto, MasterTestimonialCommand>(_testimonialsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["customername"] = dto.CustomerName,
            ["companyname"] = dto.CompanyName,
            ["comment"] = dto.Comment,
            ["star"] = dto.Star,
            ["sequenceno"] = dto.SequenceNo,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleTestimonial), "testimonial")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Testimonial", MenuName = "Master_Testimonial")]
        public IActionResult MasterAddTestimonial()
        {
            return View("MasterAddTestimonial", new MasterTestimonialCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Testimonial", MenuName = "Master_Testimonial")]
        public Task<IActionResult> MasterEditTestimonial(string? uuid)
            => EditMasterAsync(
                uuid,
                _testimonialsService,
                () => new MasterTestimonialCommand { IsActive = true },
                dto => new MasterTestimonialCommand
                {
                    UUID = dto.UUID,
                    Comment = dto.Comment,
                    SequenceNo = dto.SequenceNo,
                    CompanyName = dto.CompanyName,
                    CustomerName = dto.CustomerName,
                    Star = dto.Star,
                    FilePath = _domainResolverService.BuildAbsoluteUrl(dto.FilePath),
                    IsActive = dto.IsActive
                },
                "MasterAddTestimonial",
                nameof(MasterViewTestimonial));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Testimonial", MenuName = "Master_Testimonial")]
        public async Task<IActionResult> MasterAddTestimonial(MasterTestimonialCommand command)
        {
            if (!ModelState.IsValid)
            {

                return View("MasterAddTestimonial", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };


                if (command.Image != null)
                {

                    command.FilePath = await _fileUploadService.SaveFileAsync(
                        command.Image,
                        await GetCompanyNameAsync(),
                        "Testimonial",
                        allowedExtensions);
                }

                await _testimonialsService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Testimonial added successfully!"
                    : "Testimonial updated successfully!");

                return RedirectToAction(nameof(MasterViewTestimonial));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("MasterAddTestimonial", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Testimonial Status", MenuName = "Master_Testimonial")]
        public Task<IActionResult> ToggleTestimonial(string uuid)
        => ToggleActiveAsync<MasterTestimonialDto, MasterTestimonialCommand>(uuid, _testimonialsService, "Testimonial");

        #endregion

        #region Career
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Career List", MenuName = "Master_Career")]
        public IActionResult MasterViewCareer() => View();

        [HttpPost]
        public Task<IActionResult> GetCareers()
        => GetPagedDataAsync<MasterCareerDto, MasterCareerCommand>(_careerService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["department"] = dto.DepartmentUUID,
            ["noofposition"] = dto.NumberOfPosition,
            ["experiense"] = dto.Experience,
            ["shortdescription"] = dto.ShortDescription,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCareer), "career")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Career", MenuName = "Master_Career")]
        public async Task<IActionResult> MasterAddCareer()
        {
            var vm = new MasterCareerVM();

            // Load all dropdowns
            await LoadCareers(vm);
            vm.Career = new MasterCareerCommand { IsActive = true };
            return View("MasterAddCareer", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Career", MenuName = "Master_Career")]
        public async Task<IActionResult> MasterEditCareer(string? uuid)
        {
            var vm = new MasterCareerVM();

            // Load all dropdowns
            await LoadCareers(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                // Add new employee
                vm.Career = new MasterCareerCommand { IsActive = true };
                return View("MasterAddCareer", vm);
            }
            // Load existing employee
            var dto = await _careerService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewCareer));
            }

            vm.Career = new MasterCareerCommand
            {
                UUID = dto.UUID,
                Name = dto.Name,
                IconImage = dto.IconImage,
                ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.IconImage),
                StateUUID = dto.StateUUID,
                CountryUUID = dto.CountryUUID,
                CityUUID = dto.CityUUID,
                ShortDescription = dto.ShortDescription,
                LongDescription = dto.LongDescription,
                Experience = dto.Experience,
                NumberOfPosition = dto.NumberOfPosition,
                IsActive = dto.IsActive,
                DepartmentUUID = dto.DepartmentUUID,
                JobTypeUUID = dto.JobTypeUUID,
                SkillsUUID = dto.SkillsUUID
            };

            return View("MasterAddCareer", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Career", MenuName = "Master_Career")]
        public async Task<IActionResult> MasterAddCareer(MasterCareerVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadCareers(vm);
                    return View("MasterAddCareer", vm);
                }
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                // Handle Profile Upload
                if (vm.Career.Image != null)
                {
                    vm.Career.IconImage = await _fileUploadService.SaveFileAsync(
                        vm.Career.Image,
                        await GetCompanyNameAsync(),
                        "Career",
                        allowedExtensions);
                    vm.Career.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.Career.IconImage);
                }
                if (string.IsNullOrEmpty(vm.Career.UUID) &&
            string.IsNullOrEmpty(vm.Career.IconImage))
                {
                    ModelState.AddModelError("Employee.Profile", "Profile image is required.");
                    await LoadCareers(vm);
                    return View("MasterAddCareer", vm);
                }

                await _careerService.SaveAsync(
                    vm.Career,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Career.UUID)
                    ? "Career added successfully!"
                    : "Career updated successfully!");

                return RedirectToAction(nameof(MasterViewCareer));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadCareers(vm);
                return View("MasterAddCareer", vm);
            }
        }
        private async Task LoadCareers(MasterCareerVM vm)
        {
            var Country = await _countryService.GetDropdownAsync(x => x.Title);
            vm.Country = Country.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var state = await _stateService.GetDropdownAsync(x => x.Title);
            vm.State = state.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var city = await _cityService.GetDropdownAsync(x => x.Title);
            vm.City = city.Select(c => new SelectListItem
            {
                Value = c.UUID,
                Text = c.Title
            }).ToList();


            var department = await _departmentService.GetDropdownAsync(x => x.Title);
            vm.Department = department.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var skills = await _websiteSkillsService.GetDropdownAsync(x => x.Title);
            vm.Skills = skills.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var jobtype = await _websiteJobTypeService.GetDropdownAsync(x => x.Title);
            vm.JobType = jobtype.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

        }
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Career Status", MenuName = "Master_Career")]
        public Task<IActionResult> ToggleCareer(string uuid)
           => ToggleActiveAsync<MasterCareerDto, MasterCareerCommand>(uuid, _careerService, "Career");
        #endregion

        #region Permission Group

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Permission Group List", MenuName = "Master_Permission_Group")]
        public IActionResult MasterViewPermissionGroup() => View();

        [HttpPost]
        public Task<IActionResult> GetPermissionGroup()
        => GetPagedDataAsync<MasterPermissionGroupDto, MasterPermissionGroupCommand>(_PermissionGroupService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(TogglePermissionGroup), "Permission Group")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Permission Group", MenuName = "Master_Permission_Group")]
        public IActionResult MasterAddPermissionGroup()
        {
            return View("MasterAddPermissionGroup", new MasterPermissionGroupCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Permission Group", MenuName = "Master_Permission_Group")]
        public Task<IActionResult> MasterEditPermissionGroup(string? uuid)
            => EditMasterAsync(
                uuid,
                _PermissionGroupService,
                () => new MasterPermissionGroupCommand { IsActive = true },
                dto => new MasterPermissionGroupCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "MasterAddPermissionGroup",
                nameof(MasterViewPermissionGroup));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Permission Group", MenuName = "Master_Permission_Group")]
        public Task<IActionResult> MasterAddPermissionGroup(MasterPermissionGroupCommand command)
            => SaveMasterAsync(
                command,
                _PermissionGroupService,
                "Permission Group",
                "MasterAddPropertyGroup",
                nameof(MasterViewPermissionGroup));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Permission Group Status", MenuName = "Master_Permission_Group")]
        public Task<IActionResult> TogglePermissionGroup(string uuid)
            => ToggleActiveAsync<MasterPermissionGroupDto, MasterPermissionGroupCommand>(uuid, _PermissionGroupService, "Permission Group");

        #endregion

        #region Banner
        // Developed by Krishna (27-03-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Banner List", MenuName = "Master_Banner")]
        public IActionResult MasterViewBanner() => View();

        [HttpPost]
        public Task<IActionResult> GetBanners()
          => GetPagedDataAsync<MasterBannerDto, MasterBannerCommand>(_bannerService, dto => new Dictionary<string, object>
          {
              ["uuid"] = dto.UUID,
              ["maintitle"] = dto.MainTitle ?? "",
              ["subtitle"] = dto.SubTitle ?? "",
              ["buttontext"] = dto.ButtonText ?? "",
              ["buttonurl"] = dto.ButtonURL ?? "",
              ["image"] = !string.IsNullOrEmpty(dto.BannerImage)
                  ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.BannerImage)}' alt='Blog Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
                  : "", // ADDED
              ["sequenceno"] = dto?.SequenceNo,
              ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleBanner), "banner")
          });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Banner", MenuName = "Master_Banner")]
        public IActionResult MasterAddBanner()
        {
            return View("MasterAddBanner", new MasterBannerCommand
            {
                IsActive = true
            });
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Banner", MenuName = "Master_Banner")]
        public Task<IActionResult> MasterEditBanner(string? uuid)
           => EditMasterAsync(
               uuid,
               _bannerService,
               () => new MasterBannerCommand { IsActive = true },
               dto => new MasterBannerCommand
               {
                   UUID = dto.UUID,
                   SubTitle = dto.SubTitle,
                   MainTitle = dto.MainTitle,
                   OptionalTitle = dto.OptionalTitle,
                   ButtonURL = dto.ButtonURL,
                   ButtonText = dto.ButtonText,
                   BannerImage = dto.BannerImage,
                   ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.BannerImage),
                   SequenceNo = dto.SequenceNo,
                   IsActive = dto.IsActive
               },
               "MasterAddBanner",
               nameof(MasterViewBanner));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Banner", MenuName = "Master_Banner")]
        public async Task<IActionResult> MasterAddBanner(MasterBannerCommand command)
        {
            if (command.BannerImage == null && command.Image == null)
            {
                ModelState.AddModelError("Command.Image", "Required!");
            }
            if (!ModelState.IsValid)
            {

                return View("MasterAddBanner", command);
            }

            try
            {
                //var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg", ".mp4" };


                if (command.Image != null)
                {
                    command.BannerImage = await _fileUploadService.SaveFileAsync(
                        command.Image,
                         await GetCompanyNameAsync(),
                        "banner",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.BannerImage);
                }



                await _bannerService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Banner added successfully!"
                    : "Banner updated successfully!");

                return RedirectToAction(nameof(MasterViewBanner));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);

                return View("MasterAddBanner", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Banner Status", MenuName = "Master_Banner")]
        public Task<IActionResult> ToggleBanner(string uuid)
            => ToggleActiveAsync<MasterBannerDto, MasterBannerCommand>(uuid, _bannerService, "Banner");
        #endregion

        #region FAQ Category
        // Developed by Krishna (28-03-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master FAQ Category List", MenuName = "Master_FAQ_Category")]
        public IActionResult MasterViewFaqCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetFaqCategory()
        => GetPagedDataAsync<MasterFaqCategoryDto, MasterFaqCategoryCommand>(_faqCategoryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["icon"] = dto.Icon,
            ["shortdescription"] = dto.ShortDescription,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFaqCategory), "FAQ Category")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ Category", MenuName = "Master_FAQ_Category")]
        public IActionResult MasterAddFaqCategory() => View("MasterAddFaqCategory", new MasterFaqCategoryCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master FAQ Category", MenuName = "Master_FAQ_Category")]
        public Task<IActionResult> MasterEditFaqCategory(string? uuid)
            => EditMasterAsync(
                uuid,
                _faqCategoryService,
                () => new MasterFaqCategoryCommand { IsActive = true },
                dto => new MasterFaqCategoryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    Icon = dto.Icon,
                    ShortDescription = dto.ShortDescription,
                    IsActive = dto.IsActive
                },
                "MasterAddFaqCategory",
                nameof(MasterViewFaqCategory));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ Category", MenuName = "Master_FAQ_Category")]
        public Task<IActionResult> MasterAddFaqCategory(MasterFaqCategoryCommand command)
            => SaveMasterAsync(
                command,
                _faqCategoryService,
                "FaqCategory",
                "MasterAddFaqCategory",
                nameof(MasterViewFaqCategory));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master FAQ Category Status", MenuName = "Master_FAQ_Category")]
        public Task<IActionResult> ToggleFaqCategory(string uuid)
            => ToggleActiveAsync<MasterFaqCategoryDto, MasterFaqCategoryCommand>(uuid, _faqCategoryService, "FAQ Category");

        #endregion

        #region Blog Category
        // Developed by [Your Name] (28-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Blog Category List", MenuName = "Master_Blog_Category")]
        public IActionResult MasterViewBlogCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetBlogCategory()
            => GetPagedDataAsync<MasterBlogCategoryDto, MasterBlogCategoryCommand>(_blogCategoryService, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["title"] = dto.Title ?? "",
                ["image"] = dto.ImageUrl,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleBlogCategory), "blog category")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Blog Category", MenuName = "Master_Blog_Category")]
        public IActionResult MasterAddBlogCategory() => View("MasterAddBlogCategory", new MasterBlogCategoryCommand { IsActive = true });



        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Blog Category", MenuName = "Master_Blog_Category")]
        public async Task<IActionResult> MasterEditBlogCategory(string uuid)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                return View("MasterAddBlogCategory", new MasterBlogCategoryCommand { IsActive = true });
            }

            var dto = await _blogCategoryService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewBlogCategory));
            }

            var command = new MasterBlogCategoryCommand
            {
                UUID = dto.UUID,
                Title = dto.Title,
                ImageUrl = dto.ImageUrl,
                IsActive = dto.IsActive
            };

            return View("MasterAddBlogCategory", command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Blog Category", MenuName = "Master_Blog_Category")]
        public async Task<IActionResult> MasterAddBlogCategory(MasterBlogCategoryCommand command)
        {

            if (!ModelState.IsValid)
            {
                return View("MasterAddBlogCategory", command);
            }

            try
            {


                await _blogCategoryService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Blog Category added successfully!"
                    : "Blog Category updated successfully!");

                return RedirectToAction(nameof(MasterViewBlogCategory));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("MasterAddBlogCategory", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Blog Category Status", MenuName = "Master_Blog_Category")]
        public Task<IActionResult> ToggleBlogCategory(string uuid)
            => ToggleActiveAsync<MasterBlogCategoryDto, MasterBlogCategoryCommand>(uuid, _blogCategoryService, "Blog Category");

        #endregion

        #region Blog
        // Developed by [Your Name] (28-03-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Blog List", MenuName = "Master_Blog")]
        public IActionResult MasterViewBlog() => View();

        [HttpPost]
        public Task<IActionResult> GetBlog()
    => GetPagedDataAsync<MasterBlogDto, MasterBlogCommand>(_blogService, dto => new Dictionary<string, object>
    {
        ["uuid"] = dto.UUID,
        ["title"] = dto.Title ?? "",
        ["blogcategory"] = dto.BlogCategoryUUID ?? "",  // This will show the CATEGORY NAME (because repository maps it)
        ["blogimage"] = !string.IsNullOrEmpty(dto.BlogImageUrl)
            ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.BlogImageUrl)}' alt='Blog Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
            : "",
        ["shortdescription"] = dto.ShortDescription ?? "",
        ["blogdate"] = dto.BlogDate?.ToString("dd-MMM-yyyy") ?? "",
        ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleBlog), "blog")
    });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Blog", MenuName = "Master_Blog")]
        public async Task<IActionResult> MasterAddBlog()
        {
            var Categories = await _blogCategoryService.GetDropdownAsync(x => x.Title);

            var CategoryList = Categories.Select(c => new SelectListItem
            {
                Value = c.UUID,
                Text = c.Title
            }).ToList();

            return View("MasterAddBlog", new MasterBlogVM
            {
                Command = new MasterBlogCommand
                {
                    IsActive = true,
                },
                BlogCategories = CategoryList
            });
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Blog", MenuName = "Master_Blog")]
        public async Task<IActionResult> MasterEditBlog(string? uuid)
        {
            var Categories = await _blogCategoryService.GetDropdownAsync(x => x.Title);

            var CategoryList = Categories.Select(c => new SelectListItem
            {
                Value = c.UUID,
                Text = c.Title
            }).ToList();

            if (string.IsNullOrEmpty(uuid))
            {
                return View("MasterAddBlog", new MasterBlogVM
                {
                    Command = new MasterBlogCommand
                    {
                        IsActive = true,
                    },
                    BlogCategories = CategoryList
                });
            }

            var dto = await _blogService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewBlog));
            }

            return View("MasterAddBlog", new MasterBlogVM
            {
                Command = new MasterBlogCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    BlogImageUrl = dto.BlogImageUrl,
                    BannerImageUrl = dto.BannerImageUrl,
                    BlogUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.BlogImageUrl),
                    BannerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.BannerImageUrl),
                    ShortDescription = dto.ShortDescription,
                    BlogDate = dto.BlogDate,
                    FullDescription = dto.FullDescription,
                    BlogCategoryUUID = dto.BlogCategoryUUID,
                    IsActive = dto.IsActive
                },
                BlogCategories = CategoryList
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Blog", MenuName = "Master_Blog")]
        public async Task<IActionResult> MasterAddBlog(MasterBlogVM vm)
        {
            if (vm.Command.BannerImageUrl == null && vm.Command.BannerImageFile == null)
            {
                ModelState.AddModelError("Command.BannerImageFile", "Required!");
            }
            if (vm.Command.BlogImageUrl == null && vm.Command.BlogImageFile == null)
            {
                ModelState.AddModelError("Command.BlogImageFile", "Required!");
            }
            if (!ModelState.IsValid)
            {
                vm.BlogCategories = (await _blogCategoryService.GetDropdownAsync(x => x.Title))
                    .Select(c => new SelectListItem { Value = c.UUID, Text = c.Title }).ToList();
                return View("MasterAddBlog", vm);
            }

            try
            {
                var command = vm.Command;
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                // Handle Blog Image Upload
                if (command.BlogImageFile != null)
                {

                    command.BlogImageUrl = await _fileUploadService.SaveFileAsync(
                        command.BlogImageFile,
                        await GetCompanyNameAsync(),
                        "blogs",
                        allowedExtensions);
                    command.BlogUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.BlogImageUrl);
                }

                // Handle Banner Image Upload
                if (command.BannerImageFile != null)
                {

                    command.BannerImageUrl = await _fileUploadService.SaveFileAsync(
                        command.BannerImageFile,
                        await GetCompanyNameAsync(),
                        "blogs",
                        allowedExtensions);
                    command.BannerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.BannerImageUrl);
                }

                // Validate that at least one image URL exists
                if (string.IsNullOrEmpty(command.BlogImageUrl))
                {
                    SetErrorMessage("Blog image is required!");
                    vm.BlogCategories = (await _blogCategoryService.GetDropdownAsync(x => x.Title))
                        .Select(c => new SelectListItem { Value = c.UUID, Text = c.Title }).ToList();
                    return View("MasterAddBlog", vm);
                }

                if (string.IsNullOrEmpty(command.BannerImageUrl))
                {
                    SetErrorMessage("Banner image is required!");
                    vm.BlogCategories = (await _blogCategoryService.GetDropdownAsync(x => x.Title))
                        .Select(c => new SelectListItem { Value = c.UUID, Text = c.Title }).ToList();
                    return View("MasterAddBlog", vm);
                }

                await _blogService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Blog added successfully!"
                    : "Blog updated successfully!");

                return RedirectToAction(nameof(MasterViewBlog));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.BlogCategories = (await _blogCategoryService.GetDropdownAsync(x => x.Title))
                    .Select(c => new SelectListItem { Value = c.UUID, Text = c.Title }).ToList();
                return View("MasterAddBlog", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Blog Status", MenuName = "Master_Blog")]
        public Task<IActionResult> ToggleBlog(string uuid)
            => ToggleActiveAsync<MasterBlogDto, MasterBlogCommand>(uuid, _blogService, "Blog");
        #endregion

        #region CMS

        [HttpGet]
        public async Task<IActionResult> MasterAddCMS()
        {
            var vm = new MasterCmsVM
            {

                CmsOptions = await GetCmsOptionsAsync()
            };
            vm.Command.IsActive = true;
            return View("MasterAddCMS", vm);
        }

        private async Task<List<(string UUID, string PageTitle)>> GetCmsOptionsAsync()
        {
            try
            {
                var items = await LoadDropdownAsync<MasterCMSDto, MasterCMSCommand>(
                    _cmsService,
                    x => x.PageTitle);

                return items.Select(x => (x.Value, x.Text)).ToList();
            }
            catch
            {
                return new List<(string UUID, string PageTitle)>();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MasterAddCMS(MasterCmsVM model)
        {
            model.CmsOptions = await GetCmsOptionsAsync();

            //if (model.UploadImage == null && model.ImageFile == null)
            //{
            //    ModelState.AddModelError("ImageFile", "Image is required!");
            //}

            if (!ModelState.IsValid)
            {
                return View("MasterAddCMS", model);
            }

            try
            {
                // ✅ FIX: Extract the actual page title from CmsOptions using the UUID
                var selectedOption = model.CmsOptions.FirstOrDefault(x => x.UUID == model.Command.PageTitle);

                if (selectedOption == default)
                {
                    ModelState.AddModelError(nameof(model.Command.PageTitle), "Please select a valid page title.");
                    return View("MasterAddCMS", model);
                }

                // ✅ Use the actual PageTitle, not the UUID
                var actualPageTitle = selectedOption.PageTitle;
                var command = new MasterCMSCommand
                {

                    UUID = model.Command.UUID ?? selectedOption.UUID,  // Set UUID from selected option if null
                    PageTitle = actualPageTitle,  // ✅ Use actual title, not UUID
                    UploadImage = model.Command.UploadImage,
                    Description = model.Command.Description,
                    IsActive = model.Command.IsActive
                };


                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (model.Command.ImageFile != null)
                {
                    command.UploadImage = await _fileUploadService.SaveFileAsync(
                        model.Command.ImageFile,
                        await GetCompanyNameAsync(),
                         "cms",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.UploadImage);

                }

                await _cmsService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(model.Command.UUID)
                    ? "CMS content added successfully!"
                    : "CMS content updated successfully!");

                return RedirectToAction(nameof(MasterAddCMS));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                model.CmsOptions = await GetCmsOptionsAsync();
                return View("MasterAddCMS", model);
            }
        }

        /// <summary>
        /// AJAX endpoint for loading CMS data when dropdown selection changes
        /// This is EDIT ONLY - never used for adding new data
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> GetCMSData(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return Json(new { success = false, message = "Invalid CMS id." });

            var dto = await _cmsService.GetByUuidAsync(uuid);

            if (dto == null)
                return Json(new { success = false, message = "Record not found." });

            // ✅ FIX: Use Url.Content() to convert the stored path to a proper URL
            var imageUrl = !string.IsNullOrEmpty(dto.UploadImage)
                ? Url.Content(dto.UploadImage)
                : null;

            return Json(new
            {
                success = true,
                data = new
                {
                    uuid = dto.UUID,
                    pageTitle = dto.PageTitle,
                    description = dto.Description,
                    uploadImage = imageUrl,  // ✅ Now returns proper URL
                    isactive = dto.IsActive
                }
            });
        }

        #endregion

        #region Company Basic Data

        /// <summary>
        /// Single page management for company basic data
        /// GET: Load existing data
        /// POST: Save/Update data
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManageCompanyBasicData()
        {
            try
            {
                var dto = await _companyBasicDataService.GetFirstAsync();

                if (dto == null)
                {
                    // No data exists - return empty form for creating new record
                    return View("ManageAddCompanyBasicData", new MasterCompanyBasicDataCommand
                    {
                        IsActive = true,
                        UUID = null
                    });
                }

                var vm = new MasterCompanyBasicDataCommand
                {
                    UUID = dto.UUID,
                    CompName = dto.CompName,
                    Phone = dto.Phone,
                    EmailId = dto.EmailId,
                    GoogleMapIframe = dto.GoogleMapIframe,
                    Address = dto.Address,
                    WebsiteLogo = dto.WebsiteLogo,
                    StickyLogo = dto.StickyLogo,
                    FooterLogo = dto.FooterLogo,
                    IsIncremental = dto.IsIncremental,
                    websiteUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.WebsiteLogo),
                    stickylogoUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.StickyLogo),
                    footerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.FooterLogo),
                    IsActive = dto.IsActive ?? true

                };

                return View("ManageAddCompanyBasicData", vm);
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("ManageAddCompanyBasicData", new MasterCompanyBasicDataCommand { IsActive = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageCompanyBasicData(MasterCompanyBasicDataCommand command)
        {
            // Validate logos
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
                return View("ManageAddCompanyBasicData", command);
            }

            try
            {


                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                // Handle Website Logo Upload
                if (command.WebsiteLogoImage != null)
                {
                    command.WebsiteLogo = await _fileUploadService.SaveFileAsync(
                        command.WebsiteLogoImage,
                        await GetCompanyNameAsync(),
                        "company-logos-website",
                        allowedExtensions);
                    command.websiteUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.WebsiteLogo);
                }

                // Handle Sticky Logo Upload
                if (command.StickyLogoImage != null)
                {
                    command.StickyLogo = await _fileUploadService.SaveFileAsync(
                        command.StickyLogoImage,
                        await GetCompanyNameAsync(),
                        "company-logos-sticky",
                        allowedExtensions);
                    command.stickylogoUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.StickyLogo);
                }

                // Handle Footer Logo Upload
                if (command.FooterLogoImage != null)
                {
                    command.FooterLogo = await _fileUploadService.SaveFileAsync(
                        command.FooterLogoImage,
                        await GetCompanyNameAsync(),
                        "company-logos-footer",
                        allowedExtensions);
                    command.footerUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.FooterLogo);
                }

                await _companyBasicDataService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(command.UUID)
                    ? "Company basic data added successfully!"
                    : "Company basic data updated successfully!");

                return RedirectToAction(nameof(ManageCompanyBasicData));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("ManageAddCompanyBasicData", command);
            }
        }

        #endregion

        #region FAQ
        // Developed by Krishna (27-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master FAQ List", MenuName = "Master_FAQ")]
        public IActionResult MasterViewFaq() => View();

        [HttpPost]
        public Task<IActionResult> GetFaq()
          => GetPagedDataAsync<MasterFaqDto, MasterFaqCommand>(_faqService, dto => new Dictionary<string, object>
          {
              ["uuid"] = dto.UUID,
              ["title"] = dto.Title ?? "",
              ["category"] = dto.FAQCategoryUUID ?? "",
              ["subcategory"] = dto.FAQSubCategoryUUID ?? "",
              ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFaq), "FAQ")
          });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterAddFaq()
        {
            var vm = new MasterFaqVM();
            vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
            vm.FaqSubCategoryList = await LoadDropdownAsync(_subCategoryService, x => x.Title);
            // Populate dropdowns first so the view always has required lists
            vm.Faq = new MasterFaqCommand { IsActive = true };
            return View("MasterAddFaq", vm);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterEditFaq(string? uuid)
        {
            var vm = new MasterFaqVM();
            vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
            vm.FaqSubCategoryList = await LoadDropdownAsync(_subCategoryService, x => x.Title);
            // Populate dropdowns first so the view always has required lists

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Faq = new MasterFaqCommand { IsActive = true };
                return View("MasterAddFaq", vm);
            }

            var dto = await _faqService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewFaq));
            }

            vm.Faq = new MasterFaqCommand
            {
                UUID = dto.UUID,
                FAQCategoryUUID = dto.FAQCategoryUUID,
                FAQSubCategoryUUID = dto.FAQSubCategoryUUID,
                Title = dto.Title,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            return View("MasterAddFaq", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ", MenuName = "Master_FAQ")]
        public async Task<IActionResult> MasterAddFaq(MasterFaqVM vm)
        {
            if (vm == null)
                return BadRequest();

            // Validate inner command model
            if (!TryValidateModel(vm.Faq, nameof(vm.Faq)))
            {
                vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
                vm.FaqSubCategoryList = await LoadDropdownAsync(_subCategoryService, x => x.Title);
                // repopulate all dropdowns on validation failure
                return View("MasterAddFaq", vm);
            }

            try
            {
                await _faqService.SaveAsync(vm.Faq, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Faq.UUID)
                    ? "FAQ added successfully!"
                    : "FAQ updated successfully!");

                return RedirectToAction(nameof(MasterViewFaq));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
                vm.FaqSubCategoryList = await LoadDropdownAsync(_subCategoryService, x => x.Title);
                // Reload dropdowns on error
                return View("MasterAddFaq", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master FAQ Status", MenuName = "Master_FAQ")]
        public Task<IActionResult> ToggleFaq(string uuid)
            => ToggleActiveAsync<MasterFaqDto, MasterFaqCommand>(uuid, _faqService, "Faq");
        #endregion

        #region Social Media      

        // Developed by Krishna  (28-03-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Social Media List", MenuName = "Master_Social_Media")]
        public IActionResult MasterViewSocialMedia() => View();
        [HttpPost]
        public Task<IActionResult> GetSocialMedia()
            => GetPagedDataAsync<MasterSocialMediaDto, MasterSocialMediaCommand>(_socialMediaService, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["sequenceno"] = dto.DisplayOrder,
                ["platformName"] = dto.PlatformName,
                ["iconURL"] = dto.IconURL,
                ["profileURL"] = dto.ProfileURL,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSocialMedia), "social media")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Social Media", MenuName = "Master_Social_Media")]
        public IActionResult MasterAddSocialMedia() => View("MasterAddSocialMedia", new MasterSocialMediaCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Social Media", MenuName = "Master_Social_Media")]
        public Task<IActionResult> MasterEditSocialMedia(string? uuid)
            => EditMasterAsync(
                uuid,
                _socialMediaService,
                () => new MasterSocialMediaCommand { IsActive = true },
                dto => new MasterSocialMediaCommand
                {
                    UUID = dto.UUID,
                    DisplayOrder = dto.DisplayOrder,
                    PlatformName = dto.PlatformName,
                    IconURL = dto.IconURL,
                    ProfileURL = dto.ProfileURL,
                    IsActive = dto.IsActive
                },
                "MasterAddSocialMedia",
                nameof(MasterViewSocialMedia));
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Social Media", MenuName = "Master_Social_Media")]
        public Task<IActionResult> MasterAddSocialMedia(MasterSocialMediaCommand command)
            => SaveMasterAsync(
                command,
                _socialMediaService,
                "Social Media",
                "MasterAddSocialMedia",
                nameof(MasterViewSocialMedia));
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Social Media Status", MenuName = "Master_Social_Media")]
        public Task<IActionResult> ToggleSocialMedia(string uuid)
            => ToggleActiveAsync<MasterSocialMediaDto, MasterSocialMediaCommand>(uuid, _socialMediaService, "Social Media URL");
        #endregion

        #region FinancialYear      
        // Developed by Krishna  (03-04-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Financial Year List", MenuName = "Master_FinancialYear")]
        public IActionResult MasterViewFinancialYear() => View();

        [HttpPost]
        public Task<IActionResult> GetFinancialYear()
        => GetPagedDataAsync<MasterFinancialYearDto, MasterFinancialYearCommand>(_financialYearService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Title,
            ["startdate"] = Convert.ToDateTime(dto.StartDate).ToString("dd-MMM-yyyy"),
            ["enddate"] = Convert.ToDateTime(dto.EndDate).ToString("dd-MMM-yyyy"),
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFinancialYear), "FinancialYear")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Financial Year", MenuName = "Master_FinancialYear")]
        public Task<IActionResult> MasterEditFinancialYear(string? uuid)
            => EditMasterAsync(
                uuid,
                _financialYearService,
                () => new MasterFinancialYearCommand { IsActive = true },
                dto => new MasterFinancialYearCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    IsActive = dto.IsActive
                },
                "MasterAddFinancialYear",
                nameof(MasterViewFinancialYear));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Save", Description = "Saved Master Financial Year", MenuName = "Master_FinancialYear")]
        public Task<IActionResult> MasterAddFinancialYear(MasterFinancialYearCommand command)
            => SaveMasterAsync(
                command,
                _financialYearService,
                "FinancialYear",
                "MasterAddFinancialYear",
                nameof(MasterViewFinancialYear));

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Financial Year", MenuName = "Master_FinancialYear")]
        public IActionResult MasterAddFinancialYear()
        {
            return View("MasterAddFinancialYear", new MasterFinancialYearCommand
            {
                IsActive = true
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Financial Year Status", MenuName = "Master_FinancialYear")]
        public Task<IActionResult> ToggleFinancialYear(string uuid)
            => ToggleActiveAsync<MasterFinancialYearDto, MasterFinancialYearCommand>(uuid, _financialYearService, "FinancialYear");
        #endregion

        #region Profile
        // BUG [05-04-2026][Owner:Dixita] instead of company mobile no. it shows company email
        public async Task<IActionResult> ViewProfile()
        {
            var uuid = GetUserUUID();
            if (string.IsNullOrEmpty(uuid)) return Unauthorized();

            var employee = await _employeeService.GetByUuidAsync(uuid);
            if (employee == null) return NotFound();

            employee.RoleUUID = (await _roleService.GetByUuidAsync(employee.RoleUUID))?.Title;
            employee.DesignationUUID = (await _designationService.GetByUuidAsync(employee.DesignationUUID))?.Title;
            employee.DepartmentUUID = (await _departmentService.GetByUuidAsync(employee.DepartmentUUID))?.Title;
            employee.Profile_URL = _domainResolverService.BuildAbsoluteUrl(employee.Profile_URL);

            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var employee = await _employeeService.GetByUuidAsync(GetUserUUID());
            if (employee == null) return Unauthorized();

            return View(new ProfileVM
            {
                UUID = employee.UUID,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                UserName = employee.UserName,
                Profile_URL = employee.Profile_URL,
                ProfileUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(employee.Profile_URL)

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileVM vm)
        {
            var uuid = GetUserUUID();
            if (string.IsNullOrEmpty(uuid)) return Unauthorized();
            if (!ModelState.IsValid) return View(vm);

            var employee = await _employeeService.GetByUuidAsync(vm.UUID);
            if (employee == null) return NotFound();

            vm.Profile_URL = vm.Profile != null
                ? await _fileUploadService.SaveFileAsync(
                    vm.Profile,
                    await GetCompanyNameAsync(),
                    "Employee")
                : employee.Profile_URL;

            await _employeeService.SaveAsync(new MasterEmployeeCommand
            {
                UUID = vm.UUID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Profile_URL = vm.Profile_URL,

                // keep old values (short style)
                DepartmentUUID = employee.DepartmentUUID,
                UserName = employee.UserName,
                EmailId = employee.EmailId,
                MobileNumber = employee.MobileNumber,
                GenderUUID = employee.GenderUUID,
                RoleUUID = employee.RoleUUID,
                DesignationUUID = employee.DesignationUUID,
                HonorificUUID = employee.HonorificUUID,
                Password = employee.Password,
                IsLoginAllowed = employee.IsLoginAllowed,
                EmployeeCode = employee.EmployeeCode,
                CompanyEmailID = employee.CompanyEmailID,
                CompanyMobileNumber = employee.CompanyMobileNumber,
                IsActive = employee.IsActive

            }, uuid, Utils.GetLocalIPAddress());

            SetSuccessMessage("Profile updated successfully!");
            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

        #region Master Document
        // Developed by Utsav (1-04-2026)

        private static List<SelectListItem> GetFileTypeSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select" },
                new SelectListItem { Value = "Image", Text = "Image" },
                new SelectListItem { Value = "File", Text = "File" }
            };
        }
        protected static bool IsImagePath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".webp" || ext == ".gif";
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Document List", MenuName = "Master_Document")]
        public IActionResult MasterViewDocument() => View();

        [HttpPost]
        public Task<IActionResult> GetDocument()
        => GetPagedDataAsync<MasterDocumentDto, MasterDocumentCommand>(_documentService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["filetype"] = dto.FileType ?? "",
            ["documentUrl"] = !string.IsNullOrEmpty(dto.Path)
        ? GetAbsoluteFileUrl(dto.Path)
        : "",
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleDocument), "document")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Document", MenuName = "Master_Document")]
        public IActionResult MasterAddDocument()
        {
            var vm = new MasterDocumentVM
            {
                FileTypeList = GetFileTypeSelectList()
            };
            vm.Command.IsActive = true;
            return View("MasterAddDocument", vm);
        }


        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Document", MenuName = "Master_Document")]
        public async Task<IActionResult> MasterEditDocument(string? uuid)
        {
            var vm = new MasterDocumentVM
            {
                FileTypeList = GetFileTypeSelectList()
            };

            if (string.IsNullOrEmpty(uuid))
            {
                vm.Command.IsActive = true;
                return View("MasterAddDocument", vm);
            }

            var dto = await _documentService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewDocument));
            }

            vm.Command.UUID = dto.UUID;
            vm.Command.Title = dto.Title;
            vm.Command.FileType = dto.FileType;
            vm.Command.Path = dto.Path;
            vm.Command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Path);
            vm.Command.IsActive = dto.IsActive;

            return View("MasterAddDocument", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Document", MenuName = "Master_Document")]
        public async Task<IActionResult> MasterAddDocument(MasterDocumentVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.FileTypeList = GetFileTypeSelectList();
                return View("MasterAddDocument", vm);
            }

            if (vm.Command.Path == null && vm.Command.Image == null)
            {
                ModelState.AddModelError("Image", "Document file is required!");
                vm.FileTypeList = GetFileTypeSelectList();
                return View("MasterAddDocument", vm);
            }

            try
            {
                var command = new MasterDocumentCommand
                {
                    UUID = vm.Command.UUID,
                    Title = vm.Command.Title,
                    FileType = vm.Command.FileType,
                    Path = vm.Command.Path,
                    IsActive = vm.Command.IsActive,
                    Image = vm.Command.Image
                };

                // Conditional file extension validation based on FileType
                if (vm.Command.Image != null)
                {
                    string[] allowedExtensions = vm.Command.FileType?.ToLower() switch
                    {
                        "file" => new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx" },
                        "image" => new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" },
                        _ => new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".svg" }
                    };

                    command.Path = await _fileUploadService.SaveFileAsync(
                        vm.Command.Image,
                        await GetCompanyNameAsync(),
                        "documents",
                        allowedExtensions);
                    command.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(command.Path);
                }

                await _documentService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.Command.UUID)
                    ? "Document added successfully!"
                    : "Document updated successfully!");

                return RedirectToAction(nameof(MasterViewDocument));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.FileTypeList = GetFileTypeSelectList();
                return View("MasterAddDocument", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Document Status", MenuName = "Master_Document")]
        public Task<IActionResult> ToggleDocument(string uuid)
            => ToggleActiveAsync<MasterDocumentDto, MasterDocumentCommand>(uuid, _documentService, "Document");

        #endregion

        #region Password Policy
        // Developed by Dixita (13-04-2026)
        public async Task<IActionResult> UpdatePasswordPolicy()
        {
            var dto = await _passwordPolicyService.GetFirstAsync();

            var command = dto == null
                ? new PasswordPolicyCommand
                {
                    IsActive = true
                }
                : new PasswordPolicyCommand
                {
                    UUID = dto.UUID,
                    IsActive = dto.IsActive,
                    MinLength = dto.MinLength,
                    UpperCase = dto.UpperCase,
                    LowerCase = dto.LowerCase,
                    AllowDigit = dto.AllowDigit,
                    AllowSpecialChar = dto.AllowSpecialChar,
                    SpecialCharacters = dto.SpecialCharacters
                };

            return View("UpdatePasswordPolicy", command);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePasswordPolicy(PasswordPolicyCommand command)
        {
            if (!ModelState.IsValid)
                return View("UpdatePasswordPolicy", command);

            try
            {
                var existingCMS = await _passwordPolicyService.GetByUuidAsync(command.UUID);
                if (existingCMS == null)
                {
                    SetErrorMessage("record not found!");
                    return View("UpdatePasswordPolicy", command);
                }
                await _passwordPolicyService.SaveAsync(command, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage("Data Updated Successfully.");
                return RedirectToAction(nameof(UpdatePasswordPolicy));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                return View("UpdatePasswordPolicy", command);
            }
        }

        #endregion

        #region WL Manage Path

        // Developed by Utsav  (20-04-2026)
        // WhiteLabel Version of Manage Path

        private async Task BindWLPathAndPermissionDropdownsAsync(WLMasterManagePathVM vm)
        {
            var pathDropdown = await _WLMasterpathServices.GetDropdownAsync(x => x.Path);
            var permissionDropdown = await _wlMasterPermissionService.GetDropdownAsync(x => x.Name);

            vm.PathList = pathDropdown
                .Select(x => new SelectListItem { Value = x.UUID, Text = x.Title })
                .OrderBy(i => i.Text)
                .ToList();

            vm.PermissionList = permissionDropdown
                .Select(x => new SelectListItem { Value = x.UUID, Text = x.Title })
                .OrderBy(i => i.Text)
                .ToList();
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened WL Master Manage Path List", MenuName = "WL_Master_Manage_Path")]
        public IActionResult WLMasterViewManagePath() => View();

        [HttpPost]
        public Task<IActionResult> GetWLManagePath()
        => GetPagedDataAsync<WLMasterPathPermissionDto, WLMasterPathPermissionCommand>(_WLMasterPathPermissionService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["pathname"] = dto.PathUUID,
            ["permissionname"] = dto.PermissionUUID,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleWLManagePath), "pathpermission")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add WL Master Manage Path", MenuName = "WL_Master_Manage_Path")]
        public async Task<IActionResult> WLMasterAddManagePath()
        {
            var vm = new WLMasterManagePathVM();
            await BindWLPathAndPermissionDropdownsAsync(vm);
            vm.ManagePath = new WLMasterPathPermissionCommand { IsActive = true };
            return View("WLMasterAddManagePath", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit WL Master Manage Path", MenuName = "WL_Master_Manage_Path")]
        public async Task<IActionResult> WLMasterEditManagePath(string? uuid)
        {
            var vm = new WLMasterManagePathVM();
            await BindWLPathAndPermissionDropdownsAsync(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                vm.ManagePath = new WLMasterPathPermissionCommand { IsActive = true };
                return View("WLMasterAddManagePath", vm);
            }

            var dto = await _WLMasterPathPermissionService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(WLMasterViewManagePath));
            }

            vm.ManagePath = new WLMasterPathPermissionCommand
            {
                UUID = dto.UUID,
                PathUUID = dto.PathUUID,
                PermissionUUID = dto.PermissionUUID,
                IsActive = dto.IsActive
            };

            return View("WLMasterAddManagePath", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add WL Master Manage Path", MenuName = "WL_Master_Manage_Path")]
        public async Task<IActionResult> WLMasterAddManagePath(WLMasterManagePathVM vm)
        {
            if (vm == null)
                return BadRequest();

            if (!TryValidateModel(vm.ManagePath, nameof(vm.ManagePath)))
            {
                await BindWLPathAndPermissionDropdownsAsync(vm);
                return View("WLMasterAddManagePath", vm);
            }

            try
            {
                await _WLMasterPathPermissionService.SaveAsync(vm.ManagePath, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.ManagePath.UUID)
                    ? "WL Manage Path added successfully!"
                    : "WL Manage Path updated successfully!");

                return RedirectToAction(nameof(WLMasterViewManagePath));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await BindWLPathAndPermissionDropdownsAsync(vm);
                return View("WLMasterAddManagePath", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled WL Master Manage Path Status", MenuName = "WL_Master_Manage_Path")]
        public Task<IActionResult> ToggleWLManagePath(string uuid)
            => ToggleActiveAsync<WLMasterPathPermissionDto, WLMasterPathPermissionCommand>(uuid, _WLMasterPathPermissionService, "WL Manage Path");

        #endregion

        #region Credentials_Whatsapp
        // Developed by Utsav (20-04-2026)
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened WhatsApp Credentials List", MenuName = "Credential_Whatsapp")]
        public IActionResult MasterWhatsappCredentials() => View();

        [HttpPost]
        public Task<IActionResult> GetWhatsappCredentials()
            => GetPagedDataAsync<CredentialsWhatsappDto, CredentialsWhatsappCommand>(_credentialsWhatsappServices, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["apikey"] = dto.APIKey,
                ["mobileno"] = dto.MobileNo,
                ["sendername"] = dto.SenderName,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleWhatsappCredentials), "WhatsApp Credentials")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master WhatsApp Credentials", MenuName = "Credential_Whatsapp")]
        public IActionResult MasterAddWhatsappCredentials() => View("MasterAddWhatsappCredentials", new CredentialsWhatsappCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master WhatsApp Credentials", MenuName = "Credential_Whatsapp")]
        public Task<IActionResult> MasterEditWhatsappCredentials(string? uuid)
            => EditMasterAsync(
                uuid,
                _credentialsWhatsappServices,
                () => new CredentialsWhatsappCommand { IsActive = true },
                dto => new CredentialsWhatsappCommand
                {
                    UUID = dto.UUID,
                    APIKey = dto.APIKey,
                    AccessToken = string.Empty,
                    MobileNo = dto.MobileNo,
                    SenderName = dto.SenderName,
                    IsActive = dto.IsActive
                },
                "MasterAddWhatsappCredentials",
                nameof(MasterWhatsappCredentials));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master WhatsApp Credentials", MenuName = "Credential_Whatsapp")]
        public Task<IActionResult> AddEditWhatsappCredentials(CredentialsWhatsappCommand command)
            => SaveMasterAsync(
                command,
                _credentialsWhatsappServices,
                "WhatsApp Credentials",
                "MasterAddWhatsappCredentials",
                nameof(MasterWhatsappCredentials));
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master WhatsApp Credentials Status", MenuName = "Credential_Whatsapp")]
        public Task<IActionResult> ToggleWhatsappCredentials(string uuid)
            => ToggleActiveAsync<CredentialsWhatsappDto, CredentialsWhatsappCommand>(uuid, _credentialsWhatsappServices, "WhatsApp Credentials");
        #endregion

        #region SMS Gateway Credentials
        // Developed by Utsav (20-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened SMS Gateway Credentials List", MenuName = "Credential_SMS_Gateway")]
        public IActionResult MasterSMSGatewayCredentials() => View();

        [HttpPost]
        public Task<IActionResult> GetSMSGatewayCredentials()
            => GetPagedDataAsync<CredentialsSMSGatewayDto, CredentialsSMSGatewayCommand>(_credentialsSMSGatewayServices, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["apikey"] = dto.APIKey,
                ["senderid"] = dto.SenderId,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSMSGatewayCredentials), "SMS Gateway Credentials")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add SMS Gateway Credentials", MenuName = "Credential_SMS_Gateway")]
        public IActionResult MasterAddSMSGatewayCredentials() => View("MasterAddSMSGatewayCredentials", new CredentialsSMSGatewayCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit SMS Gateway Credentials", MenuName = "Credential_SMS_Gateway")]
        public Task<IActionResult> MasterEditSMSGatewayCredentials(string? uuid)
            => EditMasterAsync(
                uuid,
                _credentialsSMSGatewayServices,
                () => new CredentialsSMSGatewayCommand { IsActive = true },
                dto => new CredentialsSMSGatewayCommand
                {
                    UUID = dto.UUID,
                    APIKey = dto.APIKey,
                    SenderId = dto.SenderId,
                    IsActive = dto.IsActive
                },
                "MasterAddSMSGatewayCredentials",
                nameof(MasterSMSGatewayCredentials));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add SMS Gateway Credentials", MenuName = "Credential_SMS_Gateway")]
        public Task<IActionResult> AddEditSMSGatewayCredentials(CredentialsSMSGatewayCommand command)
            => SaveMasterAsync(
                command,
                _credentialsSMSGatewayServices,
                "SMS Gateway Credentials",
                "MasterAddSMSGatewayCredentials",
                nameof(MasterSMSGatewayCredentials));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled SMS Gateway Credentials Status", MenuName = "Credential_SMS_Gateway")]
        public Task<IActionResult> ToggleSMSGatewayCredentials(string uuid)
            => ToggleActiveAsync<CredentialsSMSGatewayDto, CredentialsSMSGatewayCommand>(uuid, _credentialsSMSGatewayServices, "SMS Gateway Credentials");

        #endregion

        #region FAQ Sub Category
        // Developed by Krishna (28-04-2026)

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master FAQ  Sub Category List", MenuName = "Master_FAQSubCategory")]
        public IActionResult MasterViewFaqSubCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetFaqSubCategory()
          => GetPagedDataAsync<MasterFaqSubCategoryDto, MasterFaqSubCategoryCommand>(_subCategoryService, dto => new Dictionary<string, object>
          {
              ["uuid"] = dto.UUID,
              ["category"] = dto.FAQCategoryUUID ?? "",
              ["subcategory"] = dto.Title ?? "",
              ["image"] = !string.IsNullOrEmpty(dto.Image)
          ? $"<img src='{_domainResolverService.BuildAbsoluteUrl(dto.Image)}' alt='FAQ Sub Category Image' style='width: 50px; height: 50px; object-fit: cover; border-radius: 4px;' />"
          : "",
              ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleFaqSubCategory), "FAQ Sub Category")
          });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ  Sub Category", MenuName = "Master_FAQSubCategory")]
        public async Task<IActionResult> MasterAddFaqSubCategory()
        {
            var vm = new MasterFaqSubCategoryCommand { IsActive = true };
            vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
            return View("MasterAddFaqSubCategory", vm);

        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master FAQ  Sub Category", MenuName = "Master_FAQSubCategory")]
        public async Task<IActionResult> MasterEditFaqSubCategory(string? uuid)
        {
            var vm = new MasterFaqSubCategoryCommand();

            // Populate dropdowns first so the view always has required lists
            vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);

            if (string.IsNullOrEmpty(uuid))
            {
                vm = new MasterFaqSubCategoryCommand { IsActive = true };
                return View("MasterAddFaqSubCategory", vm);
            }

            var dto = await _subCategoryService.GetByUuidAsync(uuid);

            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewFaqSubCategory));
            }


            vm.UUID = dto.UUID;
            vm.Image = dto.Image;
            vm.FAQCategoryUUID = dto.FAQCategoryUUID;
            vm.Title = dto.Title;
            vm.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(dto.Image);
            vm.IsActive = dto.IsActive;

            return View("MasterAddFaqSubCategory", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master FAQ Sub Category", MenuName = "Master_FAQSubCategory")]
        public async Task<IActionResult> MasterAddFaqSubCategory(MasterFaqSubCategoryCommand vm)
        {
            if (vm == null)
                return BadRequest();

            if (vm.ImageFile == null && string.IsNullOrEmpty(vm.Image))
            {
                ModelState.AddModelError(nameof(vm.ImageFile), "Image is required!");
            }

            if (!ModelState.IsValid)
            {
                vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
                return View("MasterAddFaqSubCategory", vm);
            }

            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };


                if (vm.ImageFile != null)
                {

                    vm.Image = await _fileUploadService.SaveFileAsync(
                        vm.ImageFile,
                        await GetCompanyNameAsync(),
                        Constants.FAQ.FaqSubCategory,
                        allowedExtensions);
                    vm.ImageUrlwithdomain = _domainResolverService.BuildAbsoluteUrl(vm.Image);
                }
                await _subCategoryService.SaveAsync(vm, GetUserUUID(), Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.UUID)
                    ? "FAQ Subcategory added successfully!"
                    : "FAQ Subcategory updated successfully!");

                return RedirectToAction(nameof(MasterViewFaqSubCategory));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                // Reload dropdowns on error
                vm.FaqCategoryList = await LoadDropdownAsync(_faqCategoryService, x => x.Title);
                return View("MasterAddFaqSubCategory", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master FAQ  Sub Category Status", MenuName = "Master_FAQSubCategory")]
        public Task<IActionResult> ToggleFaqSubCategory(string uuid)
            => ToggleActiveAsync<MasterFaqSubCategoryDto, MasterFaqSubCategoryCommand>(uuid, _subCategoryService, "Faq Sub Category");
        #endregion

        #region Customerlist
        private string GetStatusBadge(bool isActive)
        {
            return isActive
                ? "<span class='badge bg-success'>Active</span>"
                : "<span class='badge bg-danger'>Inactive</span>";
        }
        private string GetConvertToAgentButton(string uuid, bool isAgent, bool isAgentHead)
        {
            // Only show for Customer (not agent / agent head)
            if (isAgent || isAgentHead)
                return "";

            return $@"
        <button class='btn btn-sm btn-primary convert-agent-btn'
                data-id='{uuid}'
                title='Convert to Agent'>
            Convert to Agent
        </button>";
        }
        private string GetConvertToAgentHeadButton(string uuid, bool isAgentHead)
        {
            if (isAgentHead)
            {
                return "";
            }

            return $@"
        <button class='btn btn-sm btn-primary convert-agenthead-btn'
                data-id='{uuid}'>
            Convert to Agent Head
        </button>";
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Customer List", MenuName = "Master_Customer")]
        public IActionResult MasterViewCustomer() => View();
        public async Task<IActionResult> GetCustomer()
        {
            return await GetPagedDataAsync<MasterCustomerDto, MasterCustomerCommand>(
                _customerService,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["fullname"] = $"{dto.FName} {dto.MName} {dto.LName}",
                    ["email"] = dto.Email ?? null,
                    ["mobile"] = dto.Mobile ?? null,
                    ["city"] = dto.CityUUID ?? null,
                    ["industry"] = dto.IndustryUUID ?? null,
                    ["status"] = GetStatusBadge(dto.IsActive),
                    ["action"] = GetConvertToAgentButton(dto.UUID, dto.IsAgent, dto.IsAgentHead)
                },
                x => !x.IsAgent && !x.IsAgentHead
                );
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Agent List", MenuName = "Master_Customer")]
        public IActionResult MasterViewAgent() => View();
        public async Task<IActionResult> GetAgent()
        {
            return await GetPagedDataAsync<MasterCustomerDto, MasterCustomerCommand>(
                _customerService,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["fullname"] = $"{dto.FName} {dto.MName} {dto.LName}",
                    ["email"] = dto.Email ?? null,
                    ["mobile"] = dto.Mobile ?? null,
                    ["city"] = dto.CityUUID ?? null,
                    ["industry"] = dto.IndustryUUID ?? null,
                    ["status"] = GetStatusBadge(dto.IsActive),
                    ["action"] = GetConvertToAgentHeadButton(dto.UUID, dto.IsAgentHead)
                },
                x => x.IsAgent && !x.IsAgentHead
                );
        }

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Agent List", MenuName = "Master_Customer")]
        public IActionResult MasterViewAgentHead() => View();
        public async Task<IActionResult> GetAgentHead()
        {
            return await GetPagedDataAsync<MasterCustomerDto, MasterCustomerCommand>(
                _customerService,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["fullname"] = $"{dto.FName} {dto.MName} {dto.LName}",
                    ["email"] = dto.Email ?? null,
                    ["mobile"] = dto.Mobile ?? null,
                    ["city"] = dto.CityUUID ?? null,
                    ["industry"] = dto.IndustryUUID ?? null,
                    ["status"] = GetStatusBadge(dto.IsActive)
                },
                x => x.IsAgent && x.IsAgentHead
                );
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Converted Customer to Agent", MenuName = "Master_Customer")]
        public async Task<IActionResult> ConvertToAgent(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return Json(new { success = false, message = "Invalid Customer." });

                await _customerService.ConvertToAgentAsync(uuid, GetUserUUID());

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

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Converted Agent to Agenthead", MenuName = "Master_Customer")]
        public async Task<IActionResult> ConvertToAgenthead(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return Json(new { success = false, message = "Invalid Agent." });

                await _customerService.ConvertToAgentHeadAsync(uuid, GetUserUUID());

                return Json(new
                {
                    success = true,
                    message = "Converted to agenthead successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
            {
                SetErrorMessage("Invalid record.");
                return RedirectToAction(nameof(MasterViewCustomer));
            }

            var user = await _customerService.GetByUuidAsync(uuid);

            if (user == null)
            {
                SetErrorMessage("Record not found.");
                return RedirectToAction(nameof(MasterViewCustomer));
            }

            // Determine user type from database flags
            string userType = CustomerTypes.Customer;

            if (user.IsAgent && !user.IsAgentHead)
            {
                userType = CustomerTypes.Agent;
            }
            else if (user.IsAgent && user.IsAgentHead)
            {
                userType = CustomerTypes.AgentHead;
            }

            var vm = new UserDetailsVM
            {
                User = user,
                Type = userType,

                PageTitle = userType switch
                {
                    CustomerTypes.Agent => "View Agent Details",
                    CustomerTypes.AgentHead => "View Agent Head Details",
                    _ => "View Customer Details"
                },

                BackUrl = userType switch
                {
                    CustomerTypes.Agent => Url.Action(nameof(MasterViewAgent))!,
                    CustomerTypes.AgentHead => Url.Action(nameof(MasterViewAgentHead))!,
                    _ => Url.Action(nameof(MasterViewCustomer))!
                }
            };

            return View(vm);
        }
        #endregion

        #region ApiInfoFields

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Api Info Fields List", MenuName = "ApiInfoFields")]
        public IActionResult ViewApiInfoFields() => View();

        [HttpPost]
        public Task<IActionResult> GetApiInfoFields()
        => GetPagedDataAsync<ApiInfoFieldsDto, ApiInfoFieldsCommand>(_apiInfoFieldsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["infoSection"] = dto.InfoSectionUUID,
            ["sequence"] = dto.Sequence,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiInfoFields), "Api Info Fields")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Api Info Fields", MenuName = "ApiInfoFields")]
        public async Task<IActionResult> AddApiInfoFields()
        {
            var vm = new ApiInfoFieldsVM
            {
                Api = await LoadDropdownAsync(_apiService, x => x.ApiName),
                Section = await LoadDropdownAsync(_apiInfoSectionService, x => x.Title),
                ApiInfoFields = new ApiInfoFieldsCommand { IsActive = true }
            };
            return View("AddApiInfoFields", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Api Info Fields", MenuName = "ApiInfoFields")]
        public async Task<IActionResult> EditApiInfoFields(string? uuid)
        {
            var vm = new ApiInfoFieldsVM
            {
                Section = await LoadDropdownAsync(_apiInfoSectionService, x => x.Title),
                ApiInfoFields = new ApiInfoFieldsCommand { IsActive = true }
            };

            // If editing an existing mapping, load DTO and map to command inside VM
            if (!string.IsNullOrWhiteSpace(uuid))
            {
                var dto = await _apiInfoFieldsService.GetByUuidAsync(uuid);

                if (dto != null)
                {

                    vm.Section = await LoadDropdownAsync(_apiInfoSectionService, x => x.Title);
                    vm.ApiInfoFields = new ApiInfoFieldsCommand
                    {
                        UUID = dto.UUID,
                        Title = dto.Title,
                        Sequence = dto.Sequence,
                        InfoSectionUUID = dto.InfoSectionUUID,
                        IsActive = dto.IsActive
                    };
                }
            }


            // Return the VM expected by the strongly-typed view
            return View("AddApiInfoFields", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Api Info Fields", MenuName = "ApiInfoFields")]
        public async Task<IActionResult> AddApiInfoFields(ApiInfoFieldsVM vm)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    vm.Section = await LoadDropdownAsync(_apiInfoSectionService, x => x.Title);
                    return View("AddApiInfoSection", vm);
                }

                await _apiInfoFieldsService.SaveAsync(
                    vm.ApiInfoFields,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress()
                );

                SetSuccessMessage(string.IsNullOrEmpty(vm.ApiInfoFields.UUID)
                    ? "Api endpoint added successfully!"
                    : "Api endpoint updated successfully!");

                return RedirectToAction(nameof(ViewApiInfoFields));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                vm.Section = await LoadDropdownAsync(_apiInfoSectionService, x => x.Title);
                return View("AddApiInfoSection", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Api Info Fields Status", MenuName = "ApiInfoFields")]
        public Task<IActionResult> ToggleApiInfoFields(string uuid)
            => ToggleActiveAsync<ApiInfoFieldsDto, ApiInfoFieldsCommand>(uuid, _apiInfoFieldsService, "Api Info Fields");
        #endregion

        #region Policy

        private static List<SelectListItem> GetVersionTypeSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Select" },
                 new SelectListItem { Value = "minor", Text = "Minor" },
            new SelectListItem { Value = "major", Text = "Major" }
            };
        }


        [HttpPost]
        public async Task<IActionResult> GetPolicyData(string? uuid, string? title)
        {
            if (string.IsNullOrWhiteSpace(uuid) && string.IsNullOrWhiteSpace(title))
                return Json(new { success = false, message = "Invalid policy id or title." });

            try
            {
                var policies = await _masterPolicyService.GetPolicyListForRegistrationAsync();
                Upgrow.Application.DTO.Master.PolicyListDto? dto = null;

                if (!string.IsNullOrWhiteSpace(uuid))
                {
                    dto = policies?.FirstOrDefault(p => string.Equals(p.UUID, uuid, StringComparison.OrdinalIgnoreCase));
                }

                if (dto == null && !string.IsNullOrWhiteSpace(title))
                {
                    dto = policies?.FirstOrDefault(p => string.Equals(p.Title?.Trim(), title.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (dto == null)
                    return Json(new { success = false, message = "Policy not found." });

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        uuid = dto.UUID,
                        sequenceNo = dto.SequenceNo,
                        version = dto.Version,
                        policyContent = dto.PolicyContent
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [ActivityLog(ActivityType = "View", Description = "View Policy", MenuName = "Master_Policy")]
        public async Task<IActionResult> MasterViewPolicy(string? selectedUuid = null)
        {
            var vm = new UpgrowAdminPanel.Models.Master.PolicyVersionVM();

            try
            {
                var policies = await _masterPolicyService.GetPolicyListForRegistrationAsync();

                var activePolicies = (policies ?? Enumerable.Empty<Upgrow.Application.DTO.Master.PolicyListDto>())
                    .Where(p => p.IsActive == true)
                    .OrderBy(p => p.Title)
                    .ToList();

                vm.PolicyOptions = activePolicies
                    .Select(p => (p.UUID, p.Title))
                    .ToList();

                vm.PolicyList = activePolicies
                    .Select(p => new SelectListItem
                    {
                        Value = p.UUID,
                        Text = p.Title
                    })
                    .ToList();

                vm.VersionTypeList = GetVersionTypeSelectList();

                // If caller provided selectedUuid, preselect and populate command fields so page shows updated values
                var uuidToLoad = selectedUuid?.Trim();
                if (!string.IsNullOrEmpty(uuidToLoad))
                {
                    var dto = activePolicies.FirstOrDefault(p => string.Equals(p.UUID, uuidToLoad, StringComparison.OrdinalIgnoreCase));
                    if (dto != null)
                    {
                        vm.Command.UUID = dto.UUID;
                        // These properties must exist on MasterPolicyCommand (view uses them)
                        vm.Command.SequenceNo = dto.SequenceNo;
                        vm.Command.Version = dto.Version;
                        vm.Command.PolicyContent = dto.PolicyContent;
                        vm.Command.FromDate = dto.FromDate;
                        vm.Command.ToDate = dto.ToDate;
                    }
                }
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
            }

            return View("MasterViewPolicy", vm);
        }


        // Insert the following method into the Policy region of MasterController (near other Policy actions).

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Save", Description = "View Policy", MenuName = "Master_Policy")]
        public async Task<IActionResult> MasterViewPolicy(PolicyVersionVM vm)
        {
            if (vm?.Command == null)
            {
                SetErrorMessage("Invalid request.");
                return RedirectToAction(nameof(MasterViewPolicy));
            }

            if (string.IsNullOrWhiteSpace(vm.Command.UUID))
            {
                SetErrorMessage("Please select a policy.");
                return RedirectToAction(nameof(MasterViewPolicy));
            }
            if (!vm.Command.FromDate.HasValue)
            {
                SetErrorMessage("From Date is required.");
                return RedirectToAction(nameof(MasterViewPolicy));
            }
            try
            {
                await _masterPolicyService.UpdatePolicyVersionAsync(
                    vm.Command.UUID,
                    vm.Command.VersionType,
                     vm.Command.FromDate.Value,
                    vm.Command.PolicyContent,
                    vm.Command.SequenceNo);

                SetSuccessMessage("Policy updated successfully.");

                // Redirect back to GET and pass the selected UUID so the updated values are shown
                return RedirectToAction(nameof(MasterViewPolicy), new { selectedUuid = vm.Command.UUID });
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                // Keep the selection on redirect so user sees the same policy (and error message)
                return RedirectToAction(nameof(MasterViewPolicy), new { selectedUuid = vm.Command.UUID });
            }
        }

        #endregion

        #region Verification Fee      

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Verification Fee List", MenuName = "Master_VerificationFee")]
        public IActionResult MasterViewVerificationFee() => View();

        [HttpPost]

        public Task<IActionResult> GetVerificationFee()
        => GetPagedDataAsync<MasterVerificationFeeDto, MasterVerificationFeeCommand>(_verificationFeeService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["verificationtype"] = dto.VerificationType,
            ["amount"] = dto.Amount,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleVerificationFee), "Verification Fee")
        });
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Verification Fee", MenuName = "Master_VerificationFee")]
        public IActionResult MasterAddVerificationFee()
        {
            return View("MasterAddVerificationFee", new MasterVerificationFeeCommand
            {
                IsActive = true
            });
        }
        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Verification Fee", MenuName = "Master_VerificationFee")]
        public Task<IActionResult> MasterEditVerificationFee(string? uuid)
            => EditMasterAsync(
                uuid,
                _verificationFeeService,
                () => new MasterVerificationFeeCommand { IsActive = true },
                dto => new MasterVerificationFeeCommand
                {
                    UUID = dto.UUID,
                    VerificationType = dto.VerificationType,
                    Amount = dto.Amount,
                    CreatedAt = dto.CreatedAt ?? DateTimeOffset.UtcNow,
                    UpdatedAt = dto.UpdatedAt ?? DateTimeOffset.UtcNow,
                    IsActive = dto.IsActive
                },
                "MasterAddVerificationFee",
                nameof(MasterViewVerificationFee));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Verification Fee", MenuName = "Master_VerificationFee")]
        public Task<IActionResult> MasterAddVerificationFee(MasterVerificationFeeCommand command)
            => SaveMasterAsync(
                command,
                _verificationFeeService,
                "Verification Fee",
                "MasterAddVerificationFee",
                nameof(MasterViewVerificationFee));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Verification Fee Status", MenuName = "Master_VerificationFee")]
        public Task<IActionResult> ToggleVerificationFee(string uuid)
            => ToggleActiveAsync<MasterVerificationFeeDto, MasterVerificationFeeCommand>(uuid, _verificationFeeService, "Verification Fee");
        #endregion

        #region Customer Industry

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened Master Business Industry List", MenuName = "MasterBusinessIndustry")]
        public IActionResult MasterViewBusinessIndustry() => View();

        [HttpPost]
        public Task<IActionResult> GetBusinessIndustry()
        => GetPagedDataAsync<MasterBusinessIndustryDto, MasterBusinessIndustryCommand>(_businessIndustryService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["title"] = dto.Title,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleBusinessIndustry), "business industry")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Business Industry", MenuName = "MasterBusinessIndustry")]
        public IActionResult MasterAddBusinessIndustry()
        {
            return View(new MasterBusinessIndustryCommand { IsActive = true });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit Master Business Industry", MenuName = "MasterBusinessIndustry")]
        public Task<IActionResult> MasterEditBusinessIndustry(string? uuid)
            => EditMasterAsync(
                uuid,
                _businessIndustryService,
                () => new MasterBusinessIndustryCommand { IsActive = true },
                dto => new MasterBusinessIndustryCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    IsActive = dto.IsActive
                },
                "MasterAddBusinessIndustry",
                nameof(MasterViewBusinessIndustry));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master Business Industry", MenuName = "MasterBusinessIndustry")]
        public Task<IActionResult> MasterAddBusinessIndustry(MasterBusinessIndustryCommand command)
            => SaveMasterAsync(
                command,
                _businessIndustryService,
                "Business Industry",
                "MasterAddBusinessIndustry",
                nameof(MasterViewBusinessIndustry));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master Business Industry Status", MenuName = "MasterBusinessIndustry")]
        public Task<IActionResult> ToggleBusinessIndustry(string uuid)
            => ToggleActiveAsync<MasterBusinessIndustryDto, MasterBusinessIndustryCommand>(uuid, _businessIndustryService, "Business Industry");

        #endregion

        #region  AppSettings

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened App Settings List", MenuName = "AppSetting")]
        public IActionResult ViewAppSettings() => View();

        [HttpPost]
        public Task<IActionResult> GetAppSettings()
        => GetPagedDataAsync<AppSettingDto, AppSettingCommand>(_appSettingsService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["key"] = dto.Key,
            ["value"] = dto.Value,
            ["description"] = dto.Description,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleAppSetting), "App Settings")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add App Setting", MenuName = "AppSetting")]
        public IActionResult AddAppSetting()
        {
            return View("AddAppSetting", new AppSettingCommand
            {
                IsActive = true
            });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit App Setting", MenuName = "AppSetting")]
        public Task<IActionResult> EditAppSetting(string? uuid)
            => EditMasterAsync(
                uuid,
                _appSettingsService,
                () => new AppSettingCommand { IsActive = true },
                dto => new AppSettingCommand
                {
                    UUID = dto.UUID,
                    Key = dto.Key,
                    Value = dto.Value,
                    Description = dto.Description,
                    IsActive = dto.IsActive
                },
                "AddAppSetting",
                nameof(ViewAppSettings));


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add App Setting", MenuName = "AppSetting")]
        public Task<IActionResult> AddAppSetting(AppSettingCommand command)
            => SaveMasterAsync(
                command,
                _appSettingsService,
                "App Setting",
                "AddAppSetting",
                nameof(ViewAppSettings));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled App Setting Status", MenuName = "AppSetting")]
        public Task<IActionResult> ToggleAppSetting(string uuid)
            => ToggleActiveAsync<AppSettingDto, AppSettingCommand>(uuid, _appSettingsService, "App Setting");

        #endregion
    }
}