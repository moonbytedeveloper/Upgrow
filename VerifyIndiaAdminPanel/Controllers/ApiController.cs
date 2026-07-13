using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moonbyte.UI;
using Upgrow.Application;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.Api;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Services.Master;
using Upgrow.Infrastructure.Filters;
using UpgrowAdminPanel.Models;
using UpgrowAdminPanel.Models.Api;
using UpgrowAdminPanel.Models.Master;
using static UpgrowAdminPanel.Models.ApiProviderMappingVM;

namespace UpgrowAdminPanel.Controllers
{
    [ActivityLog]
    public class ApiController : BaseController
    {
        private readonly IApiComponentsService _apiComponentsService;
        private readonly IApiProviderService _apiProviderService;
        private readonly IApiProviderMappingService _apiProviderMappingService;
        private readonly IApiProviderComponentMappingService _ApiProviderComponentMappingService;
        private readonly IMasterApiService _apiService;
        private readonly IMasterApiService _masterApiService;
        private readonly IApiEndpointService _apiEndpointService;
        private readonly IProviderApisService _providerApisService;
        private readonly IDomainResolverService tenantDomainResolver;
        private readonly IApiXStatusCodesService _apiXStatusCodesService;
        private readonly IApiXCategoryService _apiXCategoryService;
        private readonly IMasterApiCategoryService _apiCategoryService;

        public ApiController(IEncryptionService encryptionService,
            IDataTableParser dataTableParser,
            IApiProviderComponentMappingService ApiProviderComponentMappingService,
            IApiProviderService apiProviderService,
            IApiProviderMappingService apiProviderMappingService,
            IApiComponentsService apiComponentsService,
            IMasterApiService masterApiService,
            IApiEndpointService apiEndpointService,
            IProviderApisService providerApisService,
            IMasterApiService apiService,
            IApiXCategoryService apiXCategoryService,
            IMasterApiCategoryService apiCategoryService,
            IApiXStatusCodesService apiXStatusCodesService,
            IDomainResolverService tenantDomainResolver) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _apiProviderService = apiProviderService;
            _apiComponentsService = apiComponentsService;
            _apiProviderService = apiProviderService;
            _masterApiService = masterApiService;
            _ApiProviderComponentMappingService = ApiProviderComponentMappingService;
            _apiProviderMappingService = apiProviderMappingService;
            _apiService = apiService;
            _apiEndpointService = apiEndpointService;
            _providerApisService = providerApisService;
            this.tenantDomainResolver = tenantDomainResolver;
            _apiXCategoryService = apiXCategoryService;
            _apiXStatusCodesService = apiXStatusCodesService;
            _apiCategoryService = apiCategoryService;
        }

        #region Api Components      

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened API Components List", MenuName = "API_Components")]
        public IActionResult ViewApiComponents() => View();

        [HttpPost]
        public Task<IActionResult> GetApiComponents()
        => GetPagedDataAsync<ApiComponentsDto, ApiComponentsCommand>(_apiComponentsService, dto => new Dictionary<string, object>

        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.Name,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiComponents), "Api Component")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Component Page", MenuName = "API_Components")]
        public IActionResult AddApiComponents() => View("AddApiComponents", new ApiComponentsCommand { IsActive = true});      

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit API Component Page", MenuName = "API_Components")]
        public Task<IActionResult> EditApiComponents(string? uuid)
            => EditMasterAsync(
                uuid,
                _apiComponentsService,
                () => new ApiComponentsCommand { IsActive = true },
                dto => new ApiComponentsCommand
                {
                    UUID = dto.UUID,
                    Name = dto.Name,
                    Code = dto.Code,
                    IsActive = dto.IsActive
                },
                "AddApiComponents",
                nameof(ViewApiComponents));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Added API Component", MenuName = "API_Components")]
        public Task<IActionResult> AddApiComponents(ApiComponentsCommand command)
            => SaveMasterAsync(
                command,
                _apiComponentsService,
                "ApiComponents",
                "AddApiComponents",
                nameof(ViewApiComponents));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled API Component Status", MenuName = "API_Components")]
        public Task<IActionResult> ToggleApiComponents(string uuid)
            => ToggleActiveAsync<ApiComponentsDto, ApiComponentsCommand>(uuid, _apiComponentsService, "Api Component");
        #endregion

        #region Api Provider      
        // Developed By : Krishna (24-03-2026)
        // Updated by Krishna (23-04-2026)
        // Remove Base URL from Provider

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened API Provider List", MenuName = "API_Provider")]
        public IActionResult ViewApiProvider() => View();

        [HttpPost]
        public Task<IActionResult> GetApiProvider()
        => GetPagedDataAsync<ApiProviderDto, ApiProviderCommand>(_apiProviderService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["name"] = dto.ProviderName,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiProvider), "Api Provider")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Provider Page", MenuName = "API_Provider")]
        public IActionResult AddApiProvider() => View("AddApiProvider", new ApiProviderCommand { IsActive = true });

        [HttpGet]
        public Task<IActionResult> EditApiProvider(string? uuid)
            => EditMasterAsync(
                uuid,
                _apiProviderService,
                () => new ApiProviderCommand { IsActive = true },
                dto => new ApiProviderCommand
                {
                    UUID = dto.UUID,
                    Code = dto.Code,
                    ProviderName = dto.ProviderName,
                    IsActive = dto.IsActive
                },
                "AddApiProvider",
                nameof(ViewApiProvider));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Added API Provider", MenuName = "API_Provider")]
        public Task<IActionResult> AddApiProvider(ApiProviderCommand command)
            => SaveMasterAsync(
                command,
                _apiProviderService,
                "Api Provider",
                "AddApiProvider",
                nameof(ViewApiProvider));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled API Provider Status", MenuName = "API_Provider")]
        public Task<IActionResult> ToggleApiProvider(string uuid)
            => ToggleActiveAsync<ApiProviderDto, ApiProviderCommand>(uuid, _apiProviderService, "ApiProvider");
        #endregion

        #region Api Provider Mapping     
        // Developed By : Krishna (25-03-2026)
        // Updated by Krishna (23-04-2026)
        // Remove Priority and IsDefault 
        private async Task BindDropdowns(ApiProviderMappingVM vm)
        {
            var apilist = await _apiService.GetAllActiveAsync();
            vm.Api = apilist.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.ApiName,
            });
            vm.IsProviderSwitchable = apilist.ToDictionary(
            x => x.UUID,
            x => x.IsProviderSwitchable);

            var api = await _apiService.GetDropdownAsync(x => x.ApiName);
            vm.Api = api.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var provider = await _apiProviderService.GetDropdownAsync(x => x.ProviderName);
            vm.Provider = provider.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var providerApis = await _providerApisService.GetAllActiveAsync();
            vm.ApiProviderMappings = providerApis
                .GroupBy(m => new { m.ApiUUID, m.ProviderUUID })
                .Select(g => new ApiProviderMapItem
                {
                    ApiUUID = g.Key.ApiUUID,
                    ProviderUUID = g.Key.ProviderUUID
                })
                .ToList();

        }

        
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened API Provider Mapping List", MenuName = "API_ProviderMapping")]
        public IActionResult ViewApiProviderMapping() => View();

        [HttpPost]
        public Task<IActionResult> GetApiProviderMapping()
        => GetPagedDataAsync<ApiProviderMappingDto, ApiProviderMappingCommand>(_apiProviderMappingService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["api"] = dto.ApiUUID,
            ["provider"] = dto.ProviderUUID,                    
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiProviderMapping), "apiprovidermapping")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Provider Mapping Page", MenuName = "API_ProviderMapping")]
        public async Task<IActionResult> AddApiProviderMapping()
        {
            var vm = new ApiProviderMappingVM
            {

                ApiProvider = new ApiProviderMappingCommand { IsActive = true }
            };

            await BindDropdowns(vm);

            // Return the VM expected by the strongly-typed view
            return View("AddApiProviderMapping", vm);
        }



        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit API Provider Mapping Page", MenuName = "API_ProviderMapping")]
        public async Task<IActionResult> EditApiProviderMapping(string? uuid)
        {
            var vm = new ApiProviderMappingVM
            {
                ApiProvider = new ApiProviderMappingCommand { IsActive = true }
            };

            // If editing an existing mapping, load DTO and map to command inside VM
            if (!string.IsNullOrWhiteSpace(uuid))
            {
                var dto = await _apiProviderMappingService.GetByUuidAsync(uuid);
                if (dto != null)
                {
                    vm.ApiProvider = new ApiProviderMappingCommand
                    {
                        UUID = dto.UUID,
                        ProviderUUID = dto.ProviderUUID,
                        ApiUUID = dto.ApiUUID,
                        IsActive = dto.IsActive
                    };
                }
            }

            // Populate dropdown lists used by the view
            await BindDropdowns(vm);

            // Return the VM expected by the strongly-typed view
            return View("AddApiProviderMapping", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Added API Provider Mapping", MenuName = "API_ProviderMapping")]
        public async Task<IActionResult> AddApiProviderMapping(ApiProviderMappingVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await BindDropdowns(vm);
                    return View("AddApiProviderMapping", vm);
                }

                // SaveAsync will handle create (when UUID is null/empty) and update (when UUID present)
                await _apiProviderMappingService.SaveAsync(
                    vm.ApiProvider,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.ApiProvider.UUID)
                    ? "Api provider mapping added successfully!"
                    : "Api provider mapping updated successfully!");

                return RedirectToAction(nameof(ViewApiProviderMapping));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await BindDropdowns(vm);
                return View("AddApiProviderMapping", vm);
            }
        }

        
        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled API Provider Mapping Status", MenuName = "API_ProviderMapping")]
        public Task<IActionResult> ToggleApiProviderMapping(string uuid)
            => ToggleActiveAsync<ApiProviderMappingDto, ApiProviderMappingCommand>(uuid, _apiProviderMappingService, "ApiProviderMapping");
        #endregion

        #region ApiProviderComponentMapping
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened API Provider Component Mapping List", MenuName = "API_ProviderComponentMapping")]
        public IActionResult ViewApiProviderComponentMapping() => View();

        [HttpPost]
        public async Task<IActionResult> GetApiProviderComponentMapping()
        {
            return await GetPagedDataAsync<ApiProviderComponentMappingDto, ApiProviderComponentMappingCommand>(
                _ApiProviderComponentMappingService,  // service instance
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["api"] = dto.ApiUUID,
                    ["component"] = dto.ComponentUUID,
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiProviderComponentMapping), "Api Provider Component Mapping")
                });
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Provider Component Mapping Page", MenuName = "API_ProviderComponentMapping")]
        public async Task<IActionResult> AddApiProviderComponentMapping()
        {
            var vm = new ApiProviderComponentMappingVM();

            // Load all dropdowns
            await LoadDropdowns(vm);
            vm.ApiComponentMapping = new ApiProviderComponentMappingCommand { IsActive = true };
            return View("AddApiProviderComponentMapping", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit API Provider Component Mapping Page", MenuName = "API_ProviderComponentMapping")]
        public async Task<IActionResult> EditApiProviderComponentMapping(string? uuid)
        {
            var vm = new ApiProviderComponentMappingVM();

            // Load all dropdowns
            await LoadDropdowns(vm);

            if (string.IsNullOrEmpty(uuid))
            {
                // Add new employee
                vm.ApiComponentMapping = new ApiProviderComponentMappingCommand { IsActive = true };
                return View("AddApiProviderComponentMapping", vm);
            }

            // Load existing employee
            var dto = await _ApiProviderComponentMappingService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(ViewApiProviderComponentMapping));
            }

            vm.ApiComponentMapping = new ApiProviderComponentMappingCommand
            {
                UUID = dto.UUID,
                ApiUUID = dto.ApiUUID,
                ComponentUUID = dto.ComponentUUID
            };

            return View("AddApiProviderComponentMapping", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Added API Provider Component Mapping", MenuName = "API_ProviderComponentMapping")]
        public async Task<IActionResult> AddApiProviderComponentMapping(ApiProviderComponentMappingVM vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdowns(vm);
                    return View("AddApiProviderComponentMapping", vm);
                }
             
                await _ApiProviderComponentMappingService.SaveAsync(
                    vm.ApiComponentMapping,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.ApiComponentMapping.UUID)
                    ? "ApiProviderComponentMapping added successfully!"
                    : "AddApiProviderComponentMapping updated successfully!");

                return RedirectToAction(nameof(ViewApiProviderComponentMapping));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await LoadDropdowns(vm);
                return View("AddApiProviderComponentMapping", vm);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled API Provider Component Mapping Status", MenuName = "API_ProviderComponentMapping")]
        public Task<IActionResult> ToggleApiProviderComponentMapping(string uuid)
           => ToggleActiveAsync<ApiProviderComponentMappingDto, ApiProviderComponentMappingCommand>(uuid, _ApiProviderComponentMappingService, "Api Provider Component Mapping");

        private async Task LoadDropdowns(ApiProviderComponentMappingVM vm)
        {
            var api = await _masterApiService.GetDropdownAsync(x => x.ApiName);
            vm.Api = api.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.Title
            }).ToList();

            var apiProvider = await _apiProviderService.GetDropdownAsync(x => x.ProviderName);

            var component = await _apiComponentsService.GetDropdownAsync(x => x.Name);
            vm.Component = component.Select(c => new SelectListItem
            {
                Value = c.UUID,
                Text = c.Title
            }).ToList();

            var providerApis = await _providerApisService.GetAllActiveAsync();
            vm.ApiProviderMappings = providerApis
                .GroupBy(m => new { m.ApiUUID, m.ProviderUUID })
                .Select(g => new ApiProviderMapsItem
                {
                    ApiUUID = g.Key.ApiUUID,
                    ProviderUUID = g.Key.ProviderUUID
                })
                .ToList();
        }
        #endregion

        #region Api Provider Endpoint     
        // Developed By : Krishna (23-04-2026)
        private async Task BindapiDropdowns(ApiEndpointVM vm)
        {
            var apilist = await _apiService.GetAllActiveAsync();
            vm.Api = apilist.Select(x => new SelectListItem
            {
                Value = x.UUID,
                Text = x.ApiName,
            });
        }
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened API Endpoint List", MenuName = "Api_Endpoint")]
        public IActionResult ViewApiEndpoint() => View();

        [HttpPost]
        public Task<IActionResult> GetApiEndpoint()
        => GetPagedDataAsync<ApiEndpointDto, ApiEndpointCommand>(_apiEndpointService, dto => new Dictionary<string, object>
        {
            ["uuid"] = dto.UUID,
            ["api"] = dto.ApiUUID,
            ["endpoint"] = dto.EndpointUrl,
            ["httpmethod"] = dto.HttpMethod,
            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiEndpoint), "Api Endpoint")
        });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Endpoint Page", MenuName = "API_Endpoint")]
        public async Task<IActionResult> AddApiEndpoint()
        {
            var vm = new ApiEndpointVM
            {
                ApiEndpoint = new ApiEndpointCommand { IsActive = true }
            };

            await BindapiDropdowns(vm);

            // Return the VM expected by the strongly-typed view
            return View("AddApiEndpoint", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit API Endpoint Page", MenuName = "API_Endpoint")]
        public async Task<IActionResult> EditApiEndpoint(string? uuid)
        {
            var vm = new ApiEndpointVM
            {
                ApiEndpoint = new ApiEndpointCommand { IsActive = true }
            };

            // If editing an existing mapping, load DTO and map to command inside VM
            if (!string.IsNullOrWhiteSpace(uuid))
            {
                var dto = await _apiEndpointService.GetByUuidAsync(uuid);
                if (dto != null)
                {
                    vm.ApiEndpoint = new ApiEndpointCommand
                    {
                        UUID = dto.UUID,
                        ApiUUID = dto.ApiUUID,
                        EndpointUrl = dto.EndpointUrl,
                        HttpMethod = dto.HttpMethod,
                        IsActive = dto.IsActive
                    };
                }
            }

            // Populate dropdown lists used by the view
            await BindapiDropdowns(vm);

            // Return the VM expected by the strongly-typed view
            return View("AddApiEndpoint", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Added API Endpoint", MenuName = "API_Endpoint")]
        public async Task<IActionResult> AddApiEndpoint(ApiEndpointVM vm)
        {
            try
            {
          

                if (!ModelState.IsValid)
                {
                    await BindapiDropdowns(vm);
                    return View("AddApiEndpoint", vm);
                }

                await _apiEndpointService.SaveAsync(
                    vm.ApiEndpoint,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress()
                );

                SetSuccessMessage(string.IsNullOrEmpty(vm.ApiEndpoint.UUID)
                    ? "Api endpoint added successfully!"
                    : "Api endpoint updated successfully!");

                return RedirectToAction(nameof(ViewApiEndpoint));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                await BindapiDropdowns(vm);
                return View("AddApiEndpoint", vm);
            }
        }


        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled API Endpoint Status", MenuName = "Api_Endpoint")]
        public Task<IActionResult> ToggleApiEndpoint(string uuid)
            => ToggleActiveAsync<ApiEndpointDto, ApiEndpointCommand>(uuid, _apiEndpointService, "Api Endpoint");
        #endregion

        #region Api X Category
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened ApiXCategory List", MenuName = "ApiXCategory")]
        public IActionResult MasterViewApiXCategory() => View();

        [HttpPost]
        public Task<IActionResult> GetApiXCategory()
            => GetPagedDataAsync<ApiXCategoryDto, ApiXCategoryCommand>(_apiXCategoryService, dto => new Dictionary<string, object>
            {
                ["uuid"] = dto.UUID,
                ["title"] = dto.Title,
                ["category"] = dto.CategoryUUID,
                ["description"] = dto.Description,
                ["icon"] = dto.Icon,
                ["sequenceNo"] = dto.SequenceNo,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiXCategory), "ApiXCategory")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add ApiXCategory", MenuName = "ApiXCategory")]
        public async Task<IActionResult> MasterAddApiXCategory()
        {
            var vm = new ApiXCategoryCommand();
            vm.IsActive = true;
            vm.ApiCategoryList = await LoadDropdownAsync(_apiCategoryService, x => x.CategoryName);

            return View("MasterAddApiXCategory", vm);
        }

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit ApiXCategory", MenuName = "ApiXCategory")]
        public async Task<IActionResult> MasterEditApiXCategory(string? uuid)
        {
            var vm = new ApiXCategoryCommand { IsActive = true };

            // Populate category dropdown
            vm.ApiCategoryList = await LoadDropdownAsync(_apiCategoryService, x => x.CategoryName);

            if (string.IsNullOrWhiteSpace(uuid))
                return View("MasterAddApiXCategory", vm);

            var dto = await _apiXCategoryService.GetByUuidAsync(uuid);
            if (dto == null)
            {
                SetErrorMessage("Record not found!");
                return RedirectToAction(nameof(MasterViewApiXCategory));
            }

            vm.UUID = dto.UUID;
            vm.Title = dto.Title;
            vm.Description = dto.Description;
            vm.Icon = dto.Icon;
            vm.SequenceNo = dto.SequenceNo;
            vm.CategoryUUID = dto.CategoryUUID;
            vm.IsActive = dto.IsActive;

            return View("MasterAddApiXCategory", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add Master ApiXCategory", MenuName = "ApiXCategory")]
        public async Task<IActionResult> MasterAddApiXCategory(ApiXCategoryCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                     
                    command.ApiCategoryList = await LoadDropdownAsync(_apiCategoryService, x => x.CategoryName);
                    return View("MasterAddApiXCategory", command);
                }

                 
                return await SaveMasterAsync(
                    command,
                    _apiXCategoryService,
                    "ApiXCategory",
                    "MasterAddApiXCategory",
                    nameof(MasterViewApiXCategory));
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.Message);
                command.ApiCategoryList = await LoadDropdownAsync(_apiCategoryService, x => x.CategoryName);
                return View("MasterAddApiXCategory", command);
            }
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled Master ApiXCategory Status", MenuName = "ApiXCategory")]
        public Task<IActionResult> ToggleApiXCategory(string uuid)
            => ToggleActiveAsync<ApiXCategoryDto, ApiXCategoryCommand>(uuid, _apiXCategoryService, "ApiXCategory");

        #endregion

        #region Api X Status Codes
        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened ApiXStatusCodes List", MenuName = "ApiXStatusCodes")]
        public IActionResult MasterViewApiXStatusCodes() => View();

        [HttpPost]
        public Task<IActionResult> GetApiXStatusCodes()
            => GetPagedDataAsync<ApiXStatusCodesDto, ApiXStatusCodesCommand>(_apiXStatusCodesService, dto => new Dictionary<string, object>
            {

                ["uuid"] = dto.UUID,
                ["title"] = dto.Title,
                ["statusCode"] = dto.StatusCode,
                ["isSuccess"] = dto.IsSuccess,
                ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiXStatusCodes), "ApiXStatusCodes")
            });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add ApiXStatusCodes", MenuName = "ApiXStatusCodes")]
        public IActionResult MasterAddApiXStatusCodes() => View("MasterAddApiXStatusCodes", new ApiXStatusCodesCommand { IsActive = true });

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Edit ApiXStatusCodes", MenuName = "ApiXStatusCodes")]
        public Task<IActionResult> MasterEditApiXStatusCodes(string? uuid)
            => EditMasterAsync(
                uuid,
                _apiXStatusCodesService,
                () => new ApiXStatusCodesCommand { IsActive = true },
                dto => new ApiXStatusCodesCommand
                {
                    UUID = dto.UUID,
                    Title = dto.Title,
                    StatusCode = dto.StatusCode,
                    IsSuccess = dto.IsSuccess,
                    IsActive = dto.IsActive
                },
                "MasterAddApiXStatusCodes",
                nameof(MasterViewApiXStatusCodes));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add ApiXStatusCodes", MenuName = "ApiXStatusCodes")]
        public Task<IActionResult> MasterAddApiXStatusCodes(ApiXStatusCodesCommand command)
            => SaveMasterAsync(
                command,
                _apiXStatusCodesService,
                "API X Status Code",
                "MasterAddApiXStatusCodes",
                nameof(MasterViewApiXStatusCodes));

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled ApiXStatusCodes Status", MenuName = "ApiXStatusCodes")]
        public Task<IActionResult> ToggleApiXStatusCodes(string uuid)
            => ToggleActiveAsync<ApiXStatusCodesDto, ApiXStatusCodesCommand>(uuid, _apiXStatusCodesService, "API X Status Code");
        #endregion
    }
}
