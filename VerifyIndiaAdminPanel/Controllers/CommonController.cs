using Microsoft.AspNetCore.Mvc;
using VerifyIndia.Application.Common.Dropdowns;
using VerifyIndia.Application.IServices;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.IServices.Common;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Application.IServices.WL;
using VerifyIndia.Application.IServices.Website;
using VerifyIndia.Application.Services;
using VerifyIndia.Application.Services.Api;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Application.Services.Website;
using VerifyIndia.Infrastructure.Filters;

namespace VerifyIndiaAdminPanel.Controllers
{
    [ActivityLog]
    public class CommonController : BaseController
    {
        private readonly IDropdownQueryService _dropdownService;
        private readonly IMasterEmailCredentialService _emailCredentialService;
        private readonly IMasterPermissionGroupService _permissionGroupService;
        private readonly IMasterCountryService _countryService;
        private readonly IMasterStateService _stateService;
        private readonly IMasterCityService _cityService;
        private readonly IMasterNomenclatureService _nomenclatureService;
        private readonly IMasterEmployeeService _employeeService;
        private readonly IMasterPathService _pathService;
        private readonly IMasterPermissionService _permissionService;
        private readonly IMasterRoleService _roleService;
        private readonly IMasterDepartmentService _departmentService;
        private readonly IMasterDesignationService _designationService;
        private readonly IMasterGenderService _genderService;
        private readonly IMasterHonorificService _honorificService;
        private readonly IMasterMenuService _menuService;
        private readonly IMasterApiService _apiService;
        private readonly IApiComponentsService _apiComponentsService;
        private readonly IProviderApisService _providerApisService;
        private readonly IApiProviderService _apiProviderService;
        private readonly IMasterApiCategoryService _apiCategoryService;
        private readonly IMasterTestimonialService _testimonialService;
        private readonly IMasterCareerService _careerService;
        private readonly IMasterBannerService _bannerService;
        private readonly IMasterFaqCategoryService _faqCategoryService;
        private readonly IMasterFaqService _faqService;
        private readonly IMasterBlogService _blogService;
        private readonly IMasterBlogCategoryService _blogCategoryService;
        private readonly IMasterSocialMediaService _socialMediaService;
        private readonly ITrustedPartnersService _trustedPartnersService;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly IKnowledgeHubCategoryService _knowledgeHubCategoryService;
        private readonly IServiceCategoryService _serviceCategoryService;
        private readonly IMasterApiService _masterApiService;
        private readonly IDomainResolverService _tenantDomainResolver;
        private readonly ITenantService _tenantService;
        public CommonController(
             IEncryptionService encryptionService,
             IDropdownQueryService dropdownService,
             IMasterEmailCredentialService emailCredentialService, 
             IMasterPermissionGroupService permissionGroupService,
             IMasterCountryService countryService,
             IMasterStateService stateService,
             IMasterCityService cityService,
             IMasterNomenclatureService nomenclatureService,
             IMasterEmployeeService employeeService,
             IMasterPathService pathService,
             IMasterPermissionService permissionService,
             IMasterRoleService roleService,
             IMasterDepartmentService departmentService,
             IMasterDesignationService designationService,
             IMasterGenderService genderService,
             IMasterHonorificService honorificService,
             IMasterMenuService menuService,
             IMasterApiService apiService,
             IProviderApisService providerApisService,
             IApiProviderService apiProviderService,
             IMasterApiCategoryService apiCategoryService,
             IMasterTestimonialService testimonialService,
             IMasterCareerService careerService,
             IMasterBannerService bannerService,
             IMasterFaqCategoryService faqCategoryService,
             IMasterFaqService faqService,
             IMasterBlogService blogService,
             IMasterBlogCategoryService blogCategoryService,
             IMasterSocialMediaService socialMediaService,
             ITrustedPartnersService trustedPartnersService,
             INewsCategoryService newsCategoryService,
             IKnowledgeHubCategoryService knowledgeHubCategoryService,
             IServiceCategoryService serviceCategoryService,
             IApiComponentsService apiComponentsService,
             IMasterApiService masterApiService,
             IDataTableParser dataTableParser,
             ITenantService tenantService,
             IDomainResolverService tenantDomainResolver) : base(dataTableParser , tenantDomainResolver, encryptionService)
        {
            _dropdownService = dropdownService;
            _emailCredentialService = emailCredentialService;
            _permissionGroupService = permissionGroupService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _nomenclatureService = nomenclatureService;
            _employeeService = employeeService;
            _pathService = pathService;
            _permissionService = permissionService;
            _roleService = roleService;
            _departmentService = departmentService;
            _designationService = designationService;
            _genderService = genderService;
            _honorificService = honorificService;
            _menuService = menuService;
            _apiService = apiService;
            _providerApisService = providerApisService;
            _apiProviderService = apiProviderService;
            _apiCategoryService = apiCategoryService;
            _testimonialService = testimonialService;
            _careerService = careerService;
            _bannerService = bannerService;
            _faqCategoryService = faqCategoryService;
            _faqService = faqService;
            _blogService = blogService;
            _blogCategoryService = blogCategoryService;
            _socialMediaService = socialMediaService;
            _trustedPartnersService = trustedPartnersService;
            _knowledgeHubCategoryService = knowledgeHubCategoryService;
            _newsCategoryService = newsCategoryService;
            _knowledgeHubCategoryService = knowledgeHubCategoryService;
            _serviceCategoryService = serviceCategoryService;
            _apiComponentsService = apiComponentsService;
            _masterApiService = masterApiService;
            _testimonialService = testimonialService;
            _tenantDomainResolver = tenantDomainResolver;
        }

        // existing POST JSON endpoint (keeps current client behavior)
        [HttpPost]
        public async Task<IActionResult> GetDropdown([FromBody] DropdownRequest request)
        {
            try
            {
                var result = await _dropdownService.GetAsync(request);
                // return { value, text } objects explicitly
                var outItems = result.Select(i => new { value = i.Value, text = i.Text }).OrderBy(x => x.text);
                return Ok(outItems);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // convenience GET (for quick testing and for scripts that may issue GETs)
        [HttpGet]
        public async Task<IActionResult> GetDropdown(string key, string parentKey = null, string parentValue = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest(new { error = "key is required" });

            try
            {
                var request = new DropdownRequest
                {
                    Key = key,
                    Parents = new Dictionary<string, string>()
                };

                if (!string.IsNullOrWhiteSpace(parentKey) && !string.IsNullOrWhiteSpace(parentValue))
                {
                    request.Parents[parentKey] = parentValue;
                }

                var result = await _dropdownService.GetAsync(request);
                var outItems = result.Select(i => new { value = i.Value, text = i.Text }).OrderBy(x => x.text);
                return Ok(outItems);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEmailCredentialName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _emailCredentialService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.EmailAddress });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPermissionGroupName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _permissionGroupService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCountryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _countryService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStateName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _stateService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCityName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _cityService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNomenclatureName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _nomenclatureService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.ModuleKey });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeeName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _employeeService.GetByUuidAsync(id);

                if (dto != null)
                {
                    var fullName = $"{dto.FirstName} {dto.LastName}".Trim();
                    return Json(new { name = fullName });
                }

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPathName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _pathService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Path });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _permissionService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _roleService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartmentName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _departmentService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignationName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _designationService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGenderName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _genderService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHonorificName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _honorificService.GetByUuidAsync(id);

                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _menuService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.MenuName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApiName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _masterApiService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.ApiName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApiCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _apiCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.CategoryName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProviderName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _apiProviderService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.ProviderName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTestimonialName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _testimonialService.GetByUuidAsync(id);
                if (dto != null)
                {
                    var displayName = $"{dto.CustomerName} - {dto.CompanyName}".Trim();
                    return Json(new { name = displayName });
                }

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCareerName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _careerService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBannerName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _bannerService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.MainTitle });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFaqCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _faqCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFaqName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _faqService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBlogCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _blogCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _blogService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSocialMediaName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _socialMediaService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.PlatformName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTrustedPartnerName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _trustedPartnersService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNewsCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _newsCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Title });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetKnowledgeHubCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _knowledgeHubCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        #region Tenant-Related Endpoints

        [HttpGet]
        public async Task<IActionResult> GetTenantName(string id)
        {
            

            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                // Parse string ID to integer (Tenant ID)
                if (int.TryParse(id, out int tenantId))
                {
                    var dto = await _tenantService.GetTenantNameByIdAsync(tenantId);
                    if (dto != null && !string.IsNullOrWhiteSpace(dto.TenantName))
                    {
                        System.Diagnostics.Debug.WriteLine($"Tenant found: {dto.TenantName}");
                        return Json(new { name = dto.TenantName });
                    }
                }
          
                return Json(new { name = id });
            }
            catch (Exception ex)
            {
               
                return Json(new { name = id });
            }
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> GetApiComponentName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _apiComponentsService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetServiceCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _serviceCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.CategoryName });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetknowledgeCategoryName(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Json(new { name = id });

            try
            {
                var dto = await _knowledgeHubCategoryService.GetByUuidAsync(id);
                if (dto != null)
                    return Json(new { name = dto.Name });

                return Json(new { name = id });
            }
            catch
            {
                return Json(new { name = id });
            }
        }
    }
}

