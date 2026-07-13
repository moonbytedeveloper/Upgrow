
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Upgrow.Application;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices;
using Upgrow.Application.IServices.AIX;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.Services.AIX;
using Upgrow.Application.Services.Api;
using Upgrow.Application.Services.Master;
using Upgrow.Infrastructure.Filters;
using UpgrowAdminPanel.Models;
using UpgrowAdminPanel.Models.Apix;

namespace UpgrowAdminPanel.Controllers
{
    public class AIXController : BaseController
    {
        private readonly IApiXVersionService _apiXVersionService;
        private readonly IDomainResolverService _tenantDomainResolver;
        private readonly IMasterApiCategoryService _masterApiCategoryService;
        private readonly IApiXCategoryService _apiXCategoryService;
        private readonly IApixHeaderService _apiXHeaderService;

        private readonly IRequestSchemaService _requestSchemaService;
        private readonly IReqResSchemaFieldsService _reqResSchemaFieldsService;
        private readonly IResponseSchemaService _responseSchemaService;
        private readonly IApiXCodeMapperService _apiXCodeMapperService;
        private readonly IApiXStatusCodesService _apiXStatusCodesService;
        private readonly IApiXCodeExampleService _apiXCodeExampleService;
        private readonly IApiXLanguageContentService _apiXLanguageContentService;
        private readonly IMasterProgrammingLanguageService _masterProgrammingLanguageService;
       // public record GetParentsRequest([property: JsonPropertyName("apiXVersionUUID")] string? ApiXVersionUUID);
        public record GetParentsRequest(
    [property: JsonPropertyName("apiXVersionUUID")] string? ApiXVersionUUID,
    [property: JsonPropertyName("schemaUUID")] string? SchemaUUID,
    [property: JsonPropertyName("isSchemaForSuccess")] bool? IsSchemaForSuccess
);


        public AIXController(IEncryptionService encryptionService,
            IDataTableParser dataTableParser,
            IApiXVersionService apiXVersionService,
            IDomainResolverService tenantDomainResolver,
            IMasterApiCategoryService masterApiCategoryService,
            IApiXCategoryService apiXCategoryService,
            IApixHeaderService apiXHeaderService,
            IRequestSchemaService requestSchemaService,
            IReqResSchemaFieldsService reqResSchemaFieldsService,
            IResponseSchemaService responseSchemaService,
            IApiXCodeMapperService apiXCodeMapperService,
            IApiXStatusCodesService apiXStatusCodesService,
            IApiXCodeExampleService apiXCodeExampleService,
            IApiXLanguageContentService apiXLanguageContentService,  
            IMasterProgrammingLanguageService masterProgrammingLanguageService) : base(dataTableParser, tenantDomainResolver, encryptionService)
        {
            _apiXVersionService = apiXVersionService;
            _tenantDomainResolver = tenantDomainResolver;
            _masterApiCategoryService = masterApiCategoryService;
            _apiXCategoryService = apiXCategoryService;
            _apiXHeaderService = apiXHeaderService;
            _requestSchemaService = requestSchemaService;
            _reqResSchemaFieldsService = reqResSchemaFieldsService;
            _responseSchemaService = responseSchemaService;
            _apiXCodeMapperService = apiXCodeMapperService;
            _apiXStatusCodesService = apiXStatusCodesService;
            _apiXCodeExampleService = apiXCodeExampleService;
            _apiXLanguageContentService = apiXLanguageContentService;
            _masterProgrammingLanguageService = masterProgrammingLanguageService;
        }

      
// Generic helper: if apiXVersionUUID is empty -> use GetPagedDataAsync (server-side).
// Otherwise load all via provided loader, filter by versionSelector and return DataTables-shaped JSON.
private async Task<IActionResult> GetPagedByVersionAsync<TDto, TCommand>(
    IMasterService<TDto, TCommand> service,
    string? apiXVersionUUID,
    Func<TDto, IDictionary<string, object>> map,
    Func<Task<IEnumerable<TDto>>> allLoader,
    Func<TDto, string?> versionSelector)
    where TDto : class
    where TCommand : class
        {
            if (string.IsNullOrWhiteSpace(apiXVersionUUID))
            {
                // preserve existing server-side paging behaviour
                return await GetPagedDataAsync<TDto, TCommand>(service, map);
            }

            var ver = apiXVersionUUID.Trim();

            var all = (await allLoader())?.ToList() ?? new List<TDto>();

            // filter by versionSelector value (trimmed)
            var filtered = all
                .Where(d =>
                {
                    try
                    {
                        var v = versionSelector(d);
                        return !string.IsNullOrWhiteSpace(v) && string.Equals(v.Trim(), ver, StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();

            // map to objects that DataTables expects
            var mapped = filtered.Select(map).ToList();

            // DataTables paging params
            int draw = 0, start = 0, length = 10;
            int.TryParse(Request.Form["draw"].FirstOrDefault(), out draw);
            int.TryParse(Request.Form["start"].FirstOrDefault(), out start);
            int.TryParse(Request.Form["length"].FirstOrDefault(), out length);

            var paged = mapped.Skip(start).Take(length).ToList();

            return Ok(new
            {
                draw,
                recordsTotal = mapped.Count,
                recordsFiltered = mapped.Count,
                data = paged
            });
        }
        public IActionResult Index()
        {
            return View();
        }
        // Add these actions inside the existing AIXController class

        [HttpGet]
        [ActivityLog(ActivityType = "View", Description = "Opened APIX Versions List", MenuName = "ApiXVersion")]
        public IActionResult ManageApiDocumentMasterList()
        {
            // Returns the list view (similar to MasterViewGender). View file: Views/AIX/ManageApiDocumentMasterList.cshtml
            return View("ManageApiDocumentMasterList");
        }

        private async Task<(bool canActivate, List<string> errors)> ValidateApiXVersionActivation(string apiXVersionUUID)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(apiXVersionUUID))
            {
                errors.Add("Invalid APIX Version.");
                return (false, errors);
            }

            // =========================
            // 1. HEADER CHECK
            // =========================
            var headers = await _apiXHeaderService.GetAllActiveAsync();
            var hasHeader = headers.Any(x =>
                string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase));

            if (!hasHeader)
                errors.Add("Header tab is empty for this version.");

            // =========================
            // 2. REQUEST SCHEMA CHECK
            // RequestSchema -> ReqResSchemaFields via SchemaUUID
            // =========================
            var requestSchemas = await _requestSchemaService.GetAllActiveAsync();

            var requestSchemaUuids = requestSchemas
                .Where(x => string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.UUID)
                .ToList();

            var fields = await _reqResSchemaFieldsService.GetAllActiveAsync();

            var hasRequestFields = fields.Any(f =>
                f.SchemaUUID != null && requestSchemaUuids.Contains(f.SchemaUUID));

            if (requestSchemaUuids.Count == 0 || !hasRequestFields)
                errors.Add("Request Schema or its fields are missing.");

            // =========================
            // 3. RESPONSE + FAILURE CHECK
            // =========================
            var responseSchemas = await _responseSchemaService.GetAllActiveAsync();

            var successSchemas = responseSchemas
                .Where(x =>
                    string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase)
                    && x.IsSchemaForSuccess == true)
                .Select(x => x.UUID)
                .ToList();

            var failureSchemas = responseSchemas
                .Where(x =>
                    string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase)
                    && x.IsSchemaForSuccess == false)
                .Select(x => x.UUID)
                .ToList();

            // must have at least 1 success + 1 failure schema
            if (successSchemas.Count == 0)
                errors.Add("At least one SUCCESS schema is required.");

            if (failureSchemas.Count == 0)
                errors.Add("At least one FAILURE schema is required.");

            var allResponseSchemaUuids = successSchemas.Concat(failureSchemas).ToList();

            var hasResponseFields = fields.Any(f =>
                f.SchemaUUID != null && allResponseSchemaUuids.Contains(f.SchemaUUID));

            if (!hasResponseFields)
                errors.Add("Response schema fields are missing.");

            // =========================
            // 4. CODE MAPPER CHECK
            // =========================
            var mappers = await _apiXCodeMapperService.GetAllActiveAsync();

            var hasCodeMapper = mappers.Any(x =>
                string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase));

            if (!hasCodeMapper)
                errors.Add("Code Mapper tab is empty.");

            // =========================
            // 5. LANGUAGE SNIPPET CHECK
            // =========================
            var snippets = await _apiXLanguageContentService.GetAllActiveAsync();

            var hasSnippet = snippets.Any(x =>
                string.Equals(x.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase));

            if (!hasSnippet)
                errors.Add("Language Snippet tab is empty.");

            return (!errors.Any(), errors);
        }

        [HttpPost]
        public Task<IActionResult> GetApiXVersions()
        {
            // Returns paged data for DataTables. Maps DTO fields to the columns used by the list view.
            return GetPagedDataAsync<ApixVersionDto, ApixVersionCommand>(
                _apiXVersionService,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["title"] = dto.Title ?? "",
                    ["versionNo"] = dto.VersionNo ?? "",
                    ["method"] = dto.ApiMethod ?? "",
                    ["path"] = dto.ApiPath ?? "",
                    // action cell: use existing helper to render toggle / edit buttons
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleApiXVersion), "ApiX Version")
                });
        }



        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Version Status", MenuName = "ApiXVersion")]
        public async Task<IActionResult> ToggleApiXVersion(string uuid)
        {
            try
            {
                var version = await _apiXVersionService.GetByUuidAsync(uuid);

                if (version == null)
                    return Ok(new { success = false, message = "Record not found" });

                bool isActivating = !version.IsActive;

                // ❌ BLOCK ACTIVATION IF RULE FAILS
                if (isActivating)
                {
                    bool canActivate = await _apiXVersionService.CanActivateAsync(uuid);

                    if (!canActivate)
                    {
                        return Ok(new
                        {
                            success = false,
                            message = "You cannot activate this record."
                        });
                    }
                }

                // toggle state
                version.IsActive = isActivating;

                // ✔ SAVE USING YOUR EXISTING PATTERN
                await _apiXVersionService.SaveAndReturnAsync(
                    new ApixVersionCommand
                    {
                        UUID = version.UUID,
                        ApiCategoryUUID = version.ApiCategoryUUID,
                        ApiXCategoryUUID = version.ApiXCategoryUUID,
                        VersionNo = version.VersionNo,
                        ApiMethod = version.ApiMethod,
                        ApiPath = version.ApiPath,
                        Title = version.Title,
                        Description = version.Description,
                        SequenceNo = version.SequenceNo,
                        IsRequestBodyRequired = version.IsRequestBodyRequired,
                        IsActive = version.IsActive
                    },
                    GetUserUUID(),
                    Utils.GetLocalIPAddress()
                );

                return Ok(new
                {
                    success = true,
                    message = isActivating ? "Activated successfully" : "Deactivated successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        //[HttpPost]
        //[ActivityLog(ActivityType = "Update", Description = "Toggled APIX Version Status", MenuName = "ApiXVersion")]
        //public Task<IActionResult> ToggleApiXVersion(string uuid)
        //{
        //    // Toggle IsActive for ApiX versions via AJAX (keeps same pattern as other master controllers)
        //    return ToggleActiveAsync<ApixVersionDto, ApixVersionCommand>(uuid, _apiXVersionService, "ApiX Version");
        //}


        // Replace the existing GET ManageApiDocumentMaster method with this improved implementation.
        // It loads the ApiX version DTO plus related headers, schemas and fields so the edit page is pre-filled.

        // Replace the existing ManageApiDocumentMaster(...) method in this file with the updated version below.
        // Only the method body is changed to load the single RequestSchema for the ApiX version so the view
        // can show the Add / Update button correctly on first render.

        [HttpGet]
        [ActivityLog(ActivityType = "Click", Description = "Clicked On Add API Provider Page", MenuName = "ApiXVersion")]
        public async Task<IActionResult> ManageApiDocumentMaster(string? uuid = null)
        {
            // Create the view-model expected by the Razor view and populate dropdowns
            var vm = new ApiXVersionVM
            {
                ApiGroup = await LoadDropdownAsync(_masterApiCategoryService, x => x.CategoryName),
                ApiCategory = await LoadDropdownAsync(_apiXCategoryService, x => x.Title),
            };
            vm.Header ??= new ApixHeaderCommand();
            vm.RequestSchema ??= new RequestSchemaCommand();
            vm.ResponseSchema ??= new ResponseSchemaCommand();
            vm.ApiXCodeMapper ??= new ApiXCodeMapperCommand();
            vm.LanguageSnippet ??= new ApiXLanguageContentCommand();

            // Populate CodeMapper dropdowns so selects render dynamically
            vm.StatusList = await LoadDropdownAsync(_apiXStatusCodesService, x => x.Title);
            vm.CodeExampleList = await LoadDropdownAsync(_apiXCodeExampleService, x => x.ErrorTitle);
            vm.ProgrammingLanguages = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);
            try
            {
                var langs = await _masterProgrammingLanguageService.GetAllActiveAsync();
                vm.ProgrammingLanguages = langs
                    .Where(l => !string.IsNullOrWhiteSpace(l.UUID))
                    .Select(l => new SelectListItem { Value = l.UUID, Text = l.Title ?? "" })
                    .ToList();
            }
            catch
            {
                vm.ProgrammingLanguages = new List<SelectListItem>();
            }

            // If an UUID was passed (edit mode) load existing data and populate nested lists/commands
            if (!string.IsNullOrWhiteSpace(uuid))
            {
                // set version UUID into commands so hidden inputs are correct
                vm.ApixVersionCommand.UUID = uuid;
                vm.RequestSchema.ApiXVersionUUID = uuid;
                vm.ResponseSchema.ApiXVersionUUID = uuid;
                vm.ApiXCodeMapper.ApiXVersionUUID = uuid;
                vm.LanguageSnippet.ApiXVersionUUID = uuid;
                // Ensure Header command exists and set its ApiXVersionUUID so the hidden input is populated
                vm.Header ??= new ApixHeaderCommand();
                vm.Header.ApiXVersionUUID = uuid;

                try
                {
                    // Load the main ApiXVersion DTO and map into the ApixVersionCommand so the main form is prefilled
                    var dto = await _apiXVersionService.GetByUuidAsync(uuid);
                    if (dto != null)
                    {
                        vm.ApixVersionCommand = new ApixVersionCommand
                        {
                            UUID = dto.UUID,
                            ApiCategoryUUID = dto.ApiCategoryUUID,
                            ApiXCategoryUUID = dto.ApiXCategoryUUID,
                            VersionNo = dto.VersionNo,
                            ApiMethod = dto.ApiMethod,
                            ApiPath = dto.ApiPath,
                            Title = dto.Title,
                            Description = dto.Description,
                            IsRequestBodyRequired = dto.IsRequestBodyRequired,
                            SequenceNo = dto.SequenceNo,
                            IsActive = dto.IsActive
                        };
                    }
                }
                catch
                {
                    // best-effort; don't block page rendering on load errors
                }

                // Load headers for this ApiXVersion
                try
                {
                    var allHeaders = await _apiXHeaderService.GetAllActiveAsync();
                    vm.Headers = allHeaders
                        .Where(h => !string.IsNullOrWhiteSpace(h.ApiXVersionUUID) &&
                                    string.Equals(h.ApiXVersionUUID, uuid, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                catch
                {
                    // ignore
                }

                // Load response/request schemas and fields for this version and populate vm.Fields and vm.ResponseSchemaList
                try
                {
                    var responseSchemas = await _responseSchemaService.GetAllActiveAsync();
                    var requestSchemas = await _requestSchemaService.GetAllActiveAsync();
                    var schemaUuids = new HashSet<string>(
                        responseSchemas.Where(s => string.Equals(s.ApiXVersionUUID, uuid, StringComparison.OrdinalIgnoreCase)).Select(s => s.UUID)
                        .Concat(requestSchemas.Where(s => string.Equals(s.ApiXVersionUUID, uuid, StringComparison.OrdinalIgnoreCase)).Select(s => s.UUID))
                        .Where(u => !string.IsNullOrWhiteSpace(u))
                        .Select(u => u!)
                    );

                    // expose response-schema list for dropdowns (CodeMapper tab)
                    vm.ResponseSchemaList = responseSchemas
                        .Where(s => string.Equals(s.ApiXVersionUUID, uuid, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(s.ApiXVersionUUID))
                        .Select(s => new SelectListItem { Value = s.UUID, Text = s.SchemaType })
                        .ToList();

                    var allFields = await _reqResSchemaFieldsService.GetAllActiveAsync();
                    var fieldsForVersion = allFields
                        .Where(f => !string.IsNullOrWhiteSpace(f.SchemaUUID) && schemaUuids.Contains(f.SchemaUUID!))
                        .OrderBy(f => f.DisplayOrder)
                        .ToList();

                    // Map fields DTO -> command model used by the view so existing rows render in field tables
                    vm.Fields = fieldsForVersion.Select(f => new ReqResSchemaFieldCommand
                    {
                        UUID = f.UUID,
                        SchemaUUID = f.SchemaUUID,
                        FieldName = f.FieldName,
                        DataType = f.DataType,
                        Examlpe = f.Examlpe,
                        DisplayOrder = (int)f.DisplayOrder,
                        AllowNull = (bool)f.AllowNull,
                        IsArray = (bool)f.IsArray,
                        ParentUUID = f.ParentUUID,
                        Constraints = f.Constraints
                    }).ToList();

                    // --- NEW: load single RequestSchema for this ApiX version (one-record-per-version model) ---
                    try
                    {
                        var singleReqSchemaDto = await _requestSchemaService.GetByVersionUuidAsync(uuid);
                        if (singleReqSchemaDto != null)
                        {
                            vm.RequestSchema = new RequestSchemaCommand
                            {
                                UUID = singleReqSchemaDto.UUID,
                                ApiXVersionUUID = singleReqSchemaDto.ApiXVersionUUID,
                                RequestType = singleReqSchemaDto.RequestType,
                                RequestJson = singleReqSchemaDto.RequestJson
                            };
                        }
                    }
                    catch
                    {
                        // ignore; not critical for page rendering
                    }
                }
                catch
                {
                    // ignore mapping errors
                }

                // Optionally load existing code-mapper entries for this version (used to prefill Code Mapper table)
                try
                {
                    var allMappers = await _apiXCodeMapperService.GetAllActiveAsync();
                    var mappersForVersion = allMappers
                        .Where(m => !string.IsNullOrWhiteSpace(m.ApiXVersionUUID) &&
                                    string.Equals(m.ApiXVersionUUID, uuid, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    // If you want to show them server-side, add a new VM collection (not required for client-side appends)
                    // Example (if VM had a property): vm.CodeMappers = mappersForVersion;
                }
                catch
                {
                    // ignore
                }
            }

            return View("ManageApiDocumentMaster", vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageApiDocumentMaster(ApiXVersionVM vm)
        {
            try
            {
                // Remove nested validation entries so saving ApixVersion isn't blocked by tab models
                var nestedPrefixes = new[] { "RequestSchema.", "Field.", "Header.", "ResponseSchema.", "ApiXCodeMapper.", "LanguageSnippet." };
                var keysToRemove = ModelState.Keys.Where(k => nestedPrefixes.Any(p => k.StartsWith(p, StringComparison.OrdinalIgnoreCase))).ToList();
                foreach (var key in keysToRemove)
                    ModelState.Remove(key);

                // If ModelState invalid now, return structured errors to help debugging
                if (!ModelState.IsValid)
                {
                    // Collect error messages
                    var errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    // Repopulate dropdowns and return view so user can fix errors
                    vm.ApiGroup = await LoadDropdownAsync(_masterApiCategoryService, x => x.CategoryName);
                    vm.ApiCategory = await LoadDropdownAsync(_apiXCategoryService, x => x.Title);
                    vm.StatusList = await LoadDropdownAsync(_apiXStatusCodesService, x => x.Title);
                    vm.CodeExampleList = await LoadDropdownAsync(_apiXCodeExampleService, x => x.ErrorTitle);
                    vm.ProgrammingLanguages = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);

                    // Add a developer-friendly error so client/devtools can inspect it
                    ModelState.AddModelError(string.Empty, "Validation failed. Inspect returned 'errors' object for details.");

                    // Return view with ModelState and errors for immediate feedback
                    Response.StatusCode = 400;
                    return View("ManageApiDocumentMaster", vm);
                }

                // Prepare command (vm.ApixVersionCommand) for save.
                // If this is an update and IsActive was not supplied, preserve existing IsActive
                if (!string.IsNullOrWhiteSpace(vm.ApixVersionCommand?.UUID))
                {
                    try
                    {
                        var existing = await _apiXVersionService.GetByUuidAsync(vm.ApixVersionCommand.UUID);
                        if (existing != null && !Request.Form.Keys.Contains("ApixVersionCommand.IsActive"))
                        {
                            vm.ApixVersionCommand.IsActive = existing.IsActive;
                        }
                    }
                    catch
                    {
                        // ignore lookup issues; continue to save
                    }
                }

                var saved = await _apiXVersionService.SaveAndReturnAsync(
                    vm.ApixVersionCommand,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                SetSuccessMessage(string.IsNullOrEmpty(vm.ApixVersionCommand.UUID)
                    ? "APIX Version added successfully!"
                    : "APIX Version updated successfully!");

                // Redirect to GET so page shows saved data and nested tabs work
                return RedirectToAction(nameof(ManageApiDocumentMaster), new { uuid = saved.UUID });
            }
            catch (Exception ex)
            {
                // Log error (if you have logger)
               

                SetErrorMessage(ex.Message);

                // repopulate dropdowns and return View so user can retry
                vm.ApiGroup = await LoadDropdownAsync(_masterApiCategoryService, x => x.CategoryName);
                vm.ApiCategory = await LoadDropdownAsync(_apiXCategoryService, x => x.Title);
                vm.StatusList = await LoadDropdownAsync(_apiXStatusCodesService, x => x.Title);
                vm.ProgrammingLanguages = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);
                vm.CodeExampleList = await LoadDropdownAsync(_apiXCodeExampleService, x => x.ErrorTitle);

                return View("ManageApiDocumentMaster", vm);
            }
        }


        #region Header

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHeader([FromForm, Bind(Prefix = "Header")] ApixHeaderCommand? cmd)
        {
            if (cmd == null)
                return BadRequest(new { error = "Invalid request" });

            // Fallback: ensure ApiXVersionUUID is present (client sets hidden input before posting)
            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
            {
                var postedVersion = Request.Form["Header.ApiXVersionUUID"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(postedVersion))
                    cmd.ApiXVersionUUID = postedVersion;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return BadRequest(new
                {
                    success = false,
                    errors
                });
            }

            // Preserve IsActive when updating; ensure new records are active by default
            //if (!string.IsNullOrWhiteSpace(cmd.UUID))
            //{
            //    try
            //    {
            //        var existing = await _apiXHeaderService.GetByUuidAsync(cmd.UUID);
            //        if (existing != null)
            //        {
            //            cmd.IsActive = existing.IsActive;
            //        }
            //    }
            //    catch
            //    {
            //        // ignore lookup errors and continue (best-effort)
            //    }
            //}
            //else
            //{
            //    cmd.IsActive = true;
            //}
            //// ===== duplicate pre-check using public wrapper =====
            //if (await _apiXHeaderService.CheckDuplicateAsync(cmd))
            //{
            //    return BadRequest(new { success = false, error = "Header already exists for this API version." });
            //}
            var saved = await _apiXHeaderService.SaveAndReturnAsync(
                cmd,
                GetUserUUID(),
                Utils.GetLocalIPAddress());

            var row = new Dictionary<string, object>
            {
                ["uuid"] = saved.UUID,
                ["fieldName"] = saved.FieldName,
                ["dataType"] = saved.DataType,
                ["fieldDetails"] = saved.FieldDetails,
                ["isRequired"] = saved.IsRequired,
                ["isActive"] = saved.IsActive,
                ["action"] = GetToggleHtml(saved.UUID, saved.IsActive, nameof(ToggleHeader), "ApiX Header")
            };

            return Ok(new
            {
                success = true,
                message = string.IsNullOrWhiteSpace(cmd.UUID)
                    ? "Header created successfully"
                    : "Header updated successfully",
                item = new
                {
                    uuid = saved.UUID,
                    fieldName = saved.FieldName,
                    dataType = saved.DataType,
                    fieldDetails = saved.FieldDetails,
                    isRequired = saved.IsRequired,
                    isActive = saved.IsActive
                },
                row
            });
        }

        [HttpPost]
        public Task<IActionResult> GetApiXHeaders([FromForm] string? apiXVersionUUID)
        {
            return GetPagedByVersionAsync<ApixHeaderDto, ApixHeaderCommand>(
                _apiXHeaderService,
                apiXVersionUUID,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["fieldName"] = dto.FieldName ?? "",
                    ["dataType"] = dto.DataType ?? "",
                    ["fieldDetails"] = dto.FieldDetails ?? "",
                    ["isRequired"] = dto.IsRequired ? "Yes" : "No",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleHeader), "ApiX Header")
                },
                // wrap service call to produce Task<IEnumerable<ApixHeaderDto>>
                async () => (IEnumerable<ApixHeaderDto>)(await _apiXHeaderService.GetAllAsync()),
                dto => dto.ApiXVersionUUID
            );
        }

        [HttpGet]
        public async Task<IActionResult> EditHeaderJson(string? uuid, string? apiXVersionUUID)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest(new { error = "uuid is required" });

            var dto = await _apiXHeaderService.GetByUuidAsync(uuid);
            if (dto == null)
                return NotFound(new { error = "not found" });

            // If caller provided an ApiXVersionUUID, ensure the header belongs to that version
            if (!string.IsNullOrWhiteSpace(apiXVersionUUID) &&
                !string.Equals(dto.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(new { error = "not found" });
            }

            // Map DTO -> command (same mapping style you use in MasterController.EditMasterAsync)
            var cmd = new ApixHeaderCommand
            {
                UUID = dto.UUID,
                ApiXVersionUUID = dto.ApiXVersionUUID,
                FieldName = dto.FieldName,
                DataType = dto.DataType,
                FieldDetails = dto.FieldDetails,
                IsRequired = dto.IsRequired,
                IsActive = dto.IsActive
            };

            // Return explicit camelCase fields so client JS (item.uuid etc.) works reliably
            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = cmd.UUID,
                    apiXVersionUUID = cmd.ApiXVersionUUID,
                    fieldName = cmd.FieldName,
                    dataType = cmd.DataType,
                    fieldDetails = cmd.FieldDetails,
                    isRequired = cmd.IsRequired,
                    isActive = cmd.IsActive
                }
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Header Status", MenuName = "ApiXHeader")]
        public Task<IActionResult> ToggleHeader(string uuid)
        {
            // Reuse existing ToggleActiveAsync pattern used elsewhere in the controller
            return ToggleActiveAsync<ApixHeaderDto, ApixHeaderCommand>(uuid, _apiXHeaderService, "ApiX Header");
        }


       
        #endregion

        #region Request Schema

        public async Task<IActionResult> GetReqResFields([FromForm] string? apiXVersionUUID, [FromForm] string? schemaUUID, [FromForm] bool? isSchemaForSuccess)
        {
            try
            {
                var postedVersion = (apiXVersionUUID ?? Request.Form["apiXVersionUUID"].FirstOrDefault())?.Trim();
                var postedSchema = (schemaUUID ?? Request.Form["schemaUUID"].FirstOrDefault())?.Trim();

                bool? postedIsSchemaForSuccess = isSchemaForSuccess;
                if (!postedIsSchemaForSuccess.HasValue)
                {
                    var val = Request.Form["isSchemaForSuccess"].FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(val) && bool.TryParse(val, out var b)) postedIsSchemaForSuccess = b;
                }

                if (string.IsNullOrWhiteSpace(postedVersion))
                {
                    return await GetPagedDataAsync<ReqResSchemaFieldDto, ReqResSchemaFieldCommand>(
                        _reqResSchemaFieldsService,
                        dto => new Dictionary<string, object>
                        {
                            ["uuid"] = dto.UUID,
                            ["schemaUUID"] = dto.SchemaUUID ?? "",
                            ["fieldName"] = dto.FieldName ?? "",
                            ["dataType"] = dto.DataType ?? "",
                            ["example"] = dto.Examlpe ?? "",
                            ["displayOrder"] = dto.DisplayOrder,
                            ["allowNull"] = (bool)dto.AllowNull ? "Yes" : "No",
                            ["isArray"] = (bool)dto.IsArray ? "Yes" : "No",
                            ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleRequestSchema), "ApiX Request")
                        });
                }

                // Load schemas using active set for dropdowns is fine; but when enumerating fields we must include inactive items
                var responseSchemas = await _responseSchemaService.GetAllActiveAsync();
                var requestSchemas = await _requestSchemaService.GetAllActiveAsync();

                var filteredResponseSchemas = new List<string>();
                if (postedIsSchemaForSuccess.HasValue)
                {
                    filteredResponseSchemas = responseSchemas
                        .Where(s =>
                            (string.IsNullOrWhiteSpace(s.ApiXVersionUUID) ||
                             string.Equals(s.ApiXVersionUUID?.Trim(), postedVersion, StringComparison.OrdinalIgnoreCase))
                            && s.IsSchemaForSuccess == postedIsSchemaForSuccess.Value)
                        .Select(s => s.UUID)
                        .Where(u => !string.IsNullOrWhiteSpace(u))
                        .ToList();
                }

                var filteredRequestSchemas = requestSchemas
                    .Where(s => string.Equals(s.ApiXVersionUUID?.Trim(), postedVersion, StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.UUID)
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .ToList();

                HashSet<string> schemaUuids;
                if (!string.IsNullOrWhiteSpace(postedSchema))
                {
                    schemaUuids = new HashSet<string>(filteredResponseSchemas.Concat(filteredRequestSchemas), StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    schemaUuids = postedIsSchemaForSuccess.HasValue
                        ? new HashSet<string>(filteredResponseSchemas, StringComparer.OrdinalIgnoreCase)
                        : new HashSet<string>(filteredRequestSchemas, StringComparer.OrdinalIgnoreCase);
                }

                // IMPORTANT: include inactive fields so toggled rows remain visible after reload
                var allFields = await _reqResSchemaFieldsService.GetAllAsync();

                var filtered = allFields.Where(f => !string.IsNullOrWhiteSpace(f.SchemaUUID) && schemaUuids.Contains(f.SchemaUUID!));

                if (!string.IsNullOrWhiteSpace(postedSchema))
                    filtered = filtered.Where(f => string.Equals(f.SchemaUUID, postedSchema, StringComparison.OrdinalIgnoreCase));

                var ordered = filtered.OrderBy(f => f.DisplayOrder).ToList();

                var mapped = ordered.Select(dto => new
                {
                    uuid = dto.UUID,
                    schemaUUID = dto.SchemaUUID ?? "",
                    fieldName = dto.FieldName ?? "",
                    dataType = dto.DataType ?? "",
                    example = dto.Examlpe ?? "",
                    displayOrder = dto.DisplayOrder,
                    allowNull = (bool)dto.AllowNull ? "Yes" : "No",
                    isArray = (bool)dto.IsArray ? "Yes" : "No",
                    action = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleSuccessResponse), "ApiX Request")
                }).ToList();

                int draw = 0;
                int.TryParse(Request.Form["draw"], out draw);

                return Ok(new
                {
                    draw,
                    recordsTotal = mapped.Count,
                    recordsFiltered = mapped.Count,
                    data = mapped
                });
            }
            catch (Exception ex)
            {
                int draw = 0;
                int.TryParse(Request.Form["draw"], out draw);

                return Ok(new
                {
                    draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "Server error while loading fields: " + ex.Message
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRequestSchemaField([FromForm, Bind(Prefix = "Field")] ReqResSchemaFieldCommand? cmd)
        {
            if (cmd == null)
                return BadRequest(new { error = "Invalid request" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return BadRequest(new
                {
                    success = false,
                    errors
                });
            }

            

            if (!string.IsNullOrWhiteSpace(cmd.UUID))
            {
                try
                {
                    var existing = await _reqResSchemaFieldsService.GetByUuidAsync(cmd.UUID);
                    if (existing != null)
                    {
                        cmd.IsActive = existing.IsActive;
                    }
                }
                catch
                {
                    // ignore lookup errors and continue (best-effort)
                }
            }
            else
            {
                // new field should be active by default
                cmd.IsActive = true;
            }
            // Duplicate pre-check using service wrapper
            try
            {
                if (await _reqResSchemaFieldsService.CheckDuplicateAsync(cmd))
                {
                    // Return field-level error so client can highlight control
                    var fieldErrors = new Dictionary<string, string[]>
            {
                { "Field.FieldName", new[] { "Field with same name already exists for selected schema." } }
            };

                    return BadRequest(new
                    {
                        success = false,
                        errors = fieldErrors
                    });
                }
            }
            catch
            {
                // On error during check, fall back to Save which will re-check and throw consistently.
            }
            var saved = await _reqResSchemaFieldsService.SaveAndReturnAsync(
                cmd,
                GetUserUUID(),
                Utils.GetLocalIPAddress());

            // Build DataTable-friendly row so client can add row immediately (no reload necessary)
            var row = new Dictionary<string, object>
            {
                ["uuid"] = saved.UUID,
                ["schemaUUID"] = saved.SchemaUUID ?? "",
                ["fieldName"] = saved.FieldName ?? "",
                ["dataType"] = saved.DataType ?? "",
                ["example"] = saved.Examlpe ?? "",
                ["displayOrder"] = saved.DisplayOrder,
                ["allowNull"] = (bool)saved.AllowNull ? "Yes" : "No",
                ["isArray"] = (bool)saved.IsArray ? "Yes" : "No",
                ["action"] = "<a href=\"javascript:void(0);\" class=\"edit-field\">Edit</a>"
            };

            return Ok(new
            {
                success = true,
                message = "Field added",
                item = new
                {
                    uuid = saved.UUID,
                    fieldName = saved.FieldName,
                    dataType = saved.DataType,
                    example = saved.Examlpe,
                    displayOrder = saved.DisplayOrder,
                    allowNull = saved.AllowNull,
                    isArray = saved.IsArray
                },
                row
            });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SaveRequestSchema([FromForm, Bind(Prefix = "RequestSchema")] RequestSchemaCommand? cmd)
        //{
        //    if (cmd == null)
        //        return BadRequest(new { success = false, error = "Invalid request" });

        //    // Fallback: if modelbinder didn't fill ApiXVersionUUID (sometimes POSTed via JS),
        //    // try to read it directly from Request.Form (keeps behavior consistent with other endpoints).
        //    if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
        //    {
        //        var postedVersion = Request.Form["RequestSchema.ApiXVersionUUID"].FirstOrDefault();
        //        if (!string.IsNullOrWhiteSpace(postedVersion))
        //            cmd.ApiXVersionUUID = postedVersion;
        //    }

        //    // If ModelState invalid -> return structured validation errors so client can show them
        //    if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState
        //            .Where(x => x.Value.Errors.Count > 0)
        //            .ToDictionary(
        //                kvp => kvp.Key,
        //                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        //            );

        //        return BadRequest(new
        //        {
        //            success = false,
        //            error = "Validation failed",
        //            errors
        //        });
        //    }

        //    if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
        //        return BadRequest(new { success = false, error = "Please save the API version first before creating request schema" });

        //    try
        //    {
        //        var saved = await _requestSchemaService.SaveAndReturnAsync(cmd, GetUserUUID(), Utils.GetLocalIPAddress());

        //        var row = new Dictionary<string, object>
        //        {
        //            ["uuid"] = saved.UUID,
        //            ["requestType"] = saved.RequestType ?? "",
        //            ["requestJson"] = saved.RequestJson ?? "",
        //            ["action"] = "<a href=\"javascript:void(0);\" class=\"edit-schema\">Edit</a>"
        //        };

        //        return Ok(new
        //        {
        //            success = true,
        //            message = "Request schema saved",
        //            schemaUuid = saved.UUID,
        //            item = new { uuid = saved.UUID, requestType = saved.RequestType, requestJson = saved.RequestJson },
        //            row
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return detailed error for debugging. Remove/modify in production.
        //        var inner = ex.InnerException;
        //        return BadRequest(new
        //        {
        //            success = false,
        //            error = "Server exception while saving RequestSchema",
        //            exception = ex.Message,
        //            innerException = inner?.Message,
        //            stackTrace = ex.ToString()
        //        });
        //    }
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRequestSchema([FromForm, Bind(Prefix = "RequestSchema")] RequestSchemaCommand? cmd)
        {
            if (cmd == null) return BadRequest(new { error = "Invalid request" });
            if (!ModelState.IsValid) return BadRequest(new { error = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)) });
            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID)) return BadRequest(new { error = "Please save the API version first before creating request schema" });

            var saved = await _requestSchemaService.SaveAndReturnAsync(cmd, GetUserUUID(), Utils.GetLocalIPAddress());

            var row = new Dictionary<string, object>
            {
                ["uuid"] = saved.UUID,
                ["requestType"] = saved.RequestType ?? "",
                ["requestJson"] = saved.RequestJson ?? "",
                ["action"] = "<a href=\"javascript:void(0);\" class=\"edit-schema\">Edit</a>"
            };

            return Ok(new
            {
                success = true,
                message = "Request schema saved",
                schemaUuid = saved.UUID,
                item = new { uuid = saved.UUID, requestType = saved.RequestType, requestJson = saved.RequestJson },
                row
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetRequestSchemaByUuid(string? uuid, string? apiXVersionUUID)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest(new { error = "uuid is required" });

            var dto = await _requestSchemaService.GetByUuidAsync(uuid);
            if (dto == null)
                return NotFound(new { error = "not found" });

            // If caller provided an ApiXVersionUUID, ensure the schema belongs to that version
            if (!string.IsNullOrWhiteSpace(apiXVersionUUID) &&
                !string.Equals(dto.ApiXVersionUUID, apiXVersionUUID, StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(new { error = "not found" });
            }

            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = dto.UUID,
                    apiXVersionUUID = dto.ApiXVersionUUID,
                    requestType = dto.RequestType,
                    requestJson = dto.RequestJson
                }
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Header Status", MenuName = "ApiXHeader")]
        public Task<IActionResult> ToggleRequestSchema(string uuid)
        {
            // Toggle active state for request/response schema fields
            return ToggleActiveAsync<ReqResSchemaFieldDto, ReqResSchemaFieldCommand>(uuid, _reqResSchemaFieldsService, "ApiX Request Field");
        }

        #endregion

        #region Response Schema
        public async Task<IActionResult> SaveResponseSchema([FromForm, Bind(Prefix = "ResponseSchema")] ResponseSchemaCommand? cmd)
        {
            if (cmd == null)
                return BadRequest(new { error = "Invalid request" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { error = string.Join("; ", errors) });
            }

            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
                return BadRequest(new { error = "Please save the API version first before creating response schema" });

            // Save into ResponseSchema (service should return saved entity with UUID)
            var saved = await _responseSchemaService.SaveAndReturnAsync(
                cmd,
                GetUserUUID(),
                Utils.GetLocalIPAddress());

            return Ok(new
            {
                success = true,
                message = "Response schema saved",
                schemaUuid = saved.UUID,
                item = new
                {
                    uuid = saved.UUID,
                    shortDescription = saved.ShortDescription,
                    schemaType = saved.SchemaType,
                    isSchemaForSuccess = saved.IsSchemaForSuccess
                }
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Request/Response Field Status", MenuName = "ApiXRequestField")]
        public Task<IActionResult> ToggleSuccessResponse(string uuid)
        {
            // Reuse the same service for toggling fields shown in request/response/failure tabs
            return ToggleActiveAsync<ReqResSchemaFieldDto, ReqResSchemaFieldCommand>(uuid, _reqResSchemaFieldsService, "ApiX Request Field");
        }

        [HttpGet]
        public async Task<IActionResult> GetResponseSchemaByUuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest(new { error = "uuid is required" });

            var dto = await _responseSchemaService.GetByUuidAsync(uuid);
            if (dto == null) return NotFound(new { error = "not found" });

            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = dto.UUID,
                    apiXVersionUUID = dto.ApiXVersionUUID,
                    schemaType = dto.SchemaType,
                    shortDescription = dto.ShortDescription,
                    isSchemaForSuccess = dto.IsSchemaForSuccess
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetFieldParents([FromBody] GetParentsRequest? request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ApiXVersionUUID))
                return BadRequest(new { error = "apiXVersionUUID is required" });

            var apiVersionUuid = request.ApiXVersionUUID;
            var schemaUUID = request.SchemaUUID;
            var isSchemaForSuccess = request.IsSchemaForSuccess;

            // load all schemas for this ApiXVersion (request & response)
            var responseSchemas = await _responseSchemaService.GetAllActiveAsync();
            var requestSchemas = await _requestSchemaService.GetAllActiveAsync();

            // determine which schema UUIDs to consider for this request
            IEnumerable<string> schemaUuidCandidates;

            if (!string.IsNullOrWhiteSpace(schemaUUID))
            {
                // explicit schema: only that schema (if it exists)
                schemaUuidCandidates = responseSchemas
                    .Where(s => string.Equals(s.UUID, schemaUUID, StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.UUID)
                    .Concat(requestSchemas
                        .Where(s => string.Equals(s.UUID, schemaUUID, StringComparison.OrdinalIgnoreCase))
                        .Select(s => s.UUID))
                    .Where(u => !string.IsNullOrWhiteSpace(u))!;
            }
            else if (isSchemaForSuccess.HasValue)
            {
                // response tab: only response schemas for this version matching success/failure
                schemaUuidCandidates = responseSchemas
                    .Where(s => string.Equals(s.ApiXVersionUUID, apiVersionUuid, StringComparison.OrdinalIgnoreCase)
                                && s.IsSchemaForSuccess == isSchemaForSuccess.Value)
                    .Select(s => s.UUID)
                    .Where(u => !string.IsNullOrWhiteSpace(u))!;
            }
            else
            {
                // request tab: only request schemas for this version
                schemaUuidCandidates = requestSchemas
                    .Where(s => string.Equals(s.ApiXVersionUUID, apiVersionUuid, StringComparison.OrdinalIgnoreCase))
                    .Select(s => s.UUID)
                    .Where(u => !string.IsNullOrWhiteSpace(u))!;
            }

            var schemaUuidsSet = new HashSet<string>(schemaUuidCandidates, StringComparer.OrdinalIgnoreCase);

            var allFields = await _reqResSchemaFieldsService.GetAllActiveAsync();
            // keep only fields that belong to the chosen schemas for this version/tab
            var fields = allFields.Where(f => !string.IsNullOrWhiteSpace(f.SchemaUUID) && schemaUuidsSet.Contains(f.SchemaUUID!)).ToList();

            // build lookup by UUID for path resolution
            var dict = fields.Where(f => !string.IsNullOrWhiteSpace(f.UUID)).ToDictionary(f => f.UUID!, f => f);

            string BuildPath(ReqResSchemaFieldDto f)
            {
                var names = new List<string>();
                var cur = f;
                var visited = new HashSet<string?>();
                while (cur != null && !string.IsNullOrWhiteSpace(cur.FieldName) && !visited.Contains(cur.UUID))
                {
                    names.Insert(0, cur.FieldName!);
                    visited.Add(cur.UUID);
                    if (string.IsNullOrWhiteSpace(cur.ParentUUID) || !dict.TryGetValue(cur.ParentUUID!, out var parentDto))
                        break;
                    cur = parentDto;
                }
                return string.Join(" -> ", names);
            }

            // pick candidates that are object/array (parents only)
            var candidates = fields
                .Where(f => !string.IsNullOrWhiteSpace(f.DataType) &&
                            (string.Equals(f.DataType, "object", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(f.DataType, "array", StringComparison.OrdinalIgnoreCase)))
                .OrderBy(f => f.FieldName)
                .ToList();

            var result = candidates
                .Where(c => !string.IsNullOrWhiteSpace(c.UUID))
                .Select(c => new { value = c.UUID, text = BuildPath(c) })
                .ToList();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFieldByUuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest(new { error = "uuid is required" });

            var dto = await _reqResSchemaFieldsService.GetByUuidAsync(uuid);
            if (dto == null) return NotFound(new { error = "not found" });

            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = dto.UUID,
                    schemaUUID = dto.SchemaUUID,
                    fieldName = dto.FieldName,
                    dataType = dto.DataType,
                    examlpe = dto.Examlpe,
                    displayOrder = dto.DisplayOrder,
                    allowNull = dto.AllowNull,
                    isArray = dto.IsArray,
                    parentUUID = dto.ParentUUID,
                    constraints = dto.Constraints
                }
            });
        }
        #endregion

        #region Code Mapper
        [HttpPost]
        public async Task<IActionResult> GetApiXCodeMappers([FromForm] string? apiXVersionUUID)
        {
            var postedVersion = (apiXVersionUUID ?? Request.Form["apiXVersionUUID"].FirstOrDefault())?.Trim();

            if (string.IsNullOrWhiteSpace(postedVersion))
            {
                return await GetPagedDataAsync<ApiXCodeMapperDto, ApiXCodeMapperCommand>(
                    _apiXCodeMapperService,
                    dto => new Dictionary<string, object>
                    {
                        ["uuid"] = dto.UUID,
                        ["statusTitle"] = dto.StatusUUID ?? "",
                        ["responseSchemaType"] = dto.ResponseSchemaUUID ?? "",
                        ["codeExampleTitle"] = dto.CodeExampleUUID ?? "",
                        ["shortDescription"] = dto.ShortDescription ?? "",
                        ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCodeMapper), "ApiX Code Mapper")
                    });
            }

            try
            {
                // include inactive mappers so they remain visible after toggle+refresh
                var allMappers = await _apiXCodeMapperService.GetAllAsync() ?? new List<ApiXCodeMapperDto>();
                string? ver = postedVersion;

                var filtered = allMappers
                    .Where(m =>
                    {
                        var mVer = m.ApiXVersionUUID?.Trim();
                        return string.IsNullOrWhiteSpace(mVer) || string.Equals(mVer, ver, StringComparison.OrdinalIgnoreCase);
                    })
                    .ToList();

                var statusList = (await LoadDropdownAsync(_apiXStatusCodesService, x => x.Title)) ?? new List<SelectListItem>();
                var codeExampleList = (await LoadDropdownAsync(_apiXCodeExampleService, x => x.ErrorTitle)) ?? new List<SelectListItem>();

                var statusDict = statusList
                    .Where(i => !string.IsNullOrWhiteSpace(i.Value))
                    .ToDictionary(i => i.Value!.Trim(), i => i.Text ?? "");

                var exampleDict = codeExampleList
                    .Where(i => !string.IsNullOrWhiteSpace(i.Value))
                    .ToDictionary(i => i.Value!.Trim(), i => i.Text ?? "");

                var responseSchemas = (await _responseSchemaService.GetAllActiveAsync()) ?? new List<ResponseSchemaDto>();

                var mapped = filtered.Select(dto =>
                {
                    string schemaType = "";
                    string statusTitle = dto.StatusUUID ?? "";
                    string exampleTitle = dto.CodeExampleUUID ?? "";

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(dto.ResponseSchemaUUID))
                        {
                            var s = responseSchemas.FirstOrDefault(r => string.Equals(r.UUID?.Trim(), dto.ResponseSchemaUUID?.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (s != null) schemaType = s.SchemaType ?? "";
                        }

                        if (!string.IsNullOrWhiteSpace(dto.StatusUUID) && statusDict.TryGetValue(dto.StatusUUID!.Trim(), out var st))
                        {
                            statusTitle = st;
                        }

                        if (!string.IsNullOrWhiteSpace(dto.CodeExampleUUID) && exampleDict.TryGetValue(dto.CodeExampleUUID!.Trim(), out var exTitle))
                        {
                            exampleTitle = exTitle;
                        }
                    }
                    catch
                    {
                        // ignore lookup errors
                    }

                    return new
                    {
                        uuid = dto.UUID,
                        statusTitle,
                        responseSchemaType = schemaType,
                        codeExampleTitle = exampleTitle,
                        shortDescription = dto.ShortDescription ?? "",
                        action = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleCodeMapper), "ApiX Code Mapper")
                    };
                }).ToList();

                int draw = 0;
                int.TryParse(Request.Form["draw"], out draw);

                return Ok(new
                {
                    draw,
                    recordsTotal = mapped.Count,
                    recordsFiltered = mapped.Count,
                    data = mapped,
                    lists = new
                    {
                        status = statusList,
                        examples = codeExampleList,
                        responseSchema = responseSchemas.Select(s => new { value = s.UUID, text = s.SchemaType })
                    }
                });
            }
            catch (Exception)
            {
                int draw = 0;
                int.TryParse(Request.Form["draw"], out draw);

                return Ok(new
                {
                    draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "Server error while loading code mappers"
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCodeMapper([FromForm] ApiXVersionVM vm)
        {
            vm ??= new ApiXVersionVM();

            // Remove validation entries for unrelated nested models so only ApiXCodeMapper is validated
            // NOTE: include LanguageSnippet and ApiXCodeMapper prefixes here — previously LanguageSnippet.ApiXVersionUUID
            // produced a ModelState invalid entry which blocked AddCodeMapper.
            var nestedPrefixes = new[] {
        "RequestSchema.",
        "Field.",
        "Header.",
        "ResponseSchema.",
        "ApiXCodeMapper.",    // ensure ApiXCodeMapper keys removed before ModelState.IsValid check
        "LanguageSnippet.",   // clear LanguageSnippet.* entries (was causing the invalid ApiXVersionUUID)
        "ApixVersionCommand.",
        "Header."
    };

            var keysToRemove = ModelState.Keys
                .Where(k => nestedPrefixes.Any(p => k.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            foreach (var key in keysToRemove)
                ModelState.Remove(key);

            // repopulate dropdowns so caller can re-render them on error
            vm.StatusList = await LoadDropdownAsync(_apiXStatusCodesService, x => x.Title);

            var responseSchemas = await _responseSchemaService.GetAllActiveAsync();
            vm.ResponseSchemaList = responseSchemas
                .Select(s => new SelectListItem { Value = s.UUID, Text = s.SchemaType })
                .ToList();

            var examples = await _apiXCodeExampleService.GetAllActiveAsync();
            vm.CodeExampleList = examples
                .Select(e => new SelectListItem { Value = e.UUID, Text = string.IsNullOrWhiteSpace(e.ErrorTitle) ? (e.Message ?? "") : e.ErrorTitle })
                .ToList();

            // Quick ModelState check (will be refined to only the nested model)
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return BadRequest(new
                {
                    success = false,
                    errors
                });
            }

            var cmd = vm?.ApiXCodeMapper;
            if (cmd == null)
            {
                return BadRequest(new
                {
                    error = "Invalid request",
                    lists = new
                    {
                        status = vm.StatusList,
                        responseSchema = vm.ResponseSchemaList,
                        examples = vm.CodeExampleList
                    }
                });
            }

            // Fallback: ensure ApiXVersionUUID is present (client sets hidden input before posting)
            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
            {
                var postedVersion = Request.Form["ApiXCodeMapper.ApiXVersionUUID"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(postedVersion))
                    cmd.ApiXVersionUUID = postedVersion;
            }

            // Ensure IsActive default behavior:
            // - If updating an existing mapper, preserve its existing IsActive
            // - If creating a new mapper, set IsActive = true by default
            if (!string.IsNullOrWhiteSpace(cmd.UUID))
            {
                try
                {
                    var existing = await _apiXCodeMapperService.GetByUuidAsync(cmd.UUID);
                    if (existing != null)
                    {
                        cmd.IsActive = existing.IsActive;
                    }
                }
                catch
                {
                    // ignore lookup errors; best-effort preservation
                }
            }
            else
            {
                cmd.IsActive = true;
            }

            // Auto-resolve ResponseSchemaUUID: match ResponseSchema.IsSchemaForSuccess == Status.IsSuccess
            if (!string.IsNullOrWhiteSpace(cmd.StatusUUID) && string.IsNullOrWhiteSpace(cmd.ResponseSchemaUUID))
            {
                try
                {
                    var statusDto = await _apiXStatusCodesService.GetByUuidAsync(cmd.StatusUUID);
                    if (statusDto != null)
                    {
                        // Prefer same ApiXVersionUUID matches; fall back to global if version not provided
                        var candidates = responseSchemas.AsEnumerable();

                        if (!string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
                        {
                            candidates = candidates.Where(s =>
                                !string.IsNullOrWhiteSpace(s.ApiXVersionUUID) &&
                                string.Equals(s.ApiXVersionUUID, cmd.ApiXVersionUUID, StringComparison.OrdinalIgnoreCase));
                        }

                        var match = candidates.FirstOrDefault(s => s.IsSchemaForSuccess == statusDto.IsSuccess);

                        // If not found with version constraint and version was provided, try without version constraint
                        if (match == null && !string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
                        {
                            match = responseSchemas.FirstOrDefault(s => s.IsSchemaForSuccess == statusDto.IsSuccess);
                        }

                        if (match != null)
                            cmd.ResponseSchemaUUID = match.UUID;
                    }
                }
                catch
                {
                    // ignore lookup errors; resolution is best-effort
                }
            }

            // Required sanity checks (ResponseSchemaUUID is attempted to be auto-resolved; keep optional if not found)
            var missing = new List<string>();
            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID)) missing.Add("ApiXVersionUUID");
            if (string.IsNullOrWhiteSpace(cmd.StatusUUID)) missing.Add("StatusUUID");
            if (string.IsNullOrWhiteSpace(cmd.CodeExampleUUID)) missing.Add("CodeExampleUUID");

            if (missing.Any())
            {
                var fieldErrors = missing.ToDictionary(
                    x => $"ApiXCodeMapper.{x}",
                    x => new[] { $"{x} is required" }
                );

                return BadRequest(new
                {
                    success = false,
                    errors = fieldErrors,
                    error = "Validation failed",
                    lists = new
                    {
                        status = vm.StatusList,
                        responseSchema = vm.ResponseSchemaList,
                        examples = vm.CodeExampleList
                    }
                });
            }

            // Validate only ApiXCodeMapper nested model
            if (!TryValidateModel(cmd, "ApiXCodeMapper"))
            {
                var errors = ModelState
                    .Where(kvp => kvp.Key.StartsWith("ApiXCodeMapper.", StringComparison.OrdinalIgnoreCase))
                    .SelectMany(kvp => kvp.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(s => !string.IsNullOrWhiteSpace(s));

                return BadRequest(new
                {
                    error = string.Join("; ", errors),
                    lists = new
                    {
                        status = vm.StatusList,
                        responseSchema = vm.ResponseSchemaList,
                        examples = vm.CodeExampleList
                    }
                });
            }
            try
            {
                if (await _apiXCodeMapperService.CheckDuplicateAsync(cmd))
                {
                    var fieldErrors = new Dictionary<string, string[]>
        {
            { "ApiXCodeMapper.ResponseSchemaUUID", new[] { "A code mapper with the same response schema already exists." } }
        };

                    return BadRequest(new
                    {
                        success = false,
                        errors = fieldErrors,
                        lists = new
                        {
                            status = vm.StatusList,
                            responseSchema = vm.ResponseSchemaList,
                            examples = vm.CodeExampleList
                        }
                    });
                }
            }
            catch
            {
                // On error during check, continue — SaveAndReturnAsync will still enforce server-side rules if present.
            }

            try
            {
                var saved = await _apiXCodeMapperService.SaveAndReturnAsync(
                    cmd,
                    GetUserUUID(),
                    Utils.GetLocalIPAddress());

                // Resolve friendly schema type
                string? schemaType = null;
                try
                {
                    if (!string.IsNullOrWhiteSpace(saved.ResponseSchemaUUID))
                    {
                        var schemaDto = await _responseSchemaService.GetByUuidAsync(saved.ResponseSchemaUUID);
                        schemaType = schemaDto?.SchemaType;
                    }
                }
                catch { /* ignore */ }

                // Resolve friendly status title
                string statusTitle = "";
                try
                {
                    if (!string.IsNullOrWhiteSpace(saved.StatusUUID))
                    {
                        var statusDto = await _apiXStatusCodesService.GetByUuidAsync(saved.StatusUUID);
                        if (statusDto != null && !string.IsNullOrWhiteSpace(statusDto.Title))
                        {
                            statusTitle = statusDto.Title;
                        }
                        else
                        {
                            statusTitle = vm.StatusList?.FirstOrDefault(i => i.Value == saved.StatusUUID)?.Text ?? saved.StatusUUID ?? "";
                        }
                    }
                }
                catch { /* ignore */ }

                // Resolve friendly code example title
                string exampleTitle = "";
                try
                {
                    if (!string.IsNullOrWhiteSpace(saved.CodeExampleUUID))
                    {
                        var exDto = await _apiXCodeExampleService.GetByUuidAsync(saved.CodeExampleUUID);
                        if (exDto != null)
                        {
                            exampleTitle = !string.IsNullOrWhiteSpace(exDto.ErrorTitle) ? exDto.ErrorTitle : exDto.Message ?? "";
                        }
                        if (string.IsNullOrWhiteSpace(exampleTitle))
                        {
                            exampleTitle = vm.CodeExampleList?.FirstOrDefault(i => i.Value == saved.CodeExampleUUID)?.Text ?? saved.CodeExampleUUID ?? "";
                        }
                    }
                }
                catch { /* ignore */ }

                // Build DataTable-friendly row (use friendly titles)
                var row = new Dictionary<string, object>
                {
                    ["uuid"] = saved.UUID,
                    ["statusTitle"] = statusTitle,
                    ["responseSchemaType"] = schemaType ?? "",
                    ["codeExampleTitle"] = exampleTitle,
                    ["shortDescription"] = saved.ShortDescription ?? "",
                    ["action"] = "<a href=\"javascript:void(0);\" class=\"edit-codemapper\">Edit</a>"
                };

                return Ok(new
                {
                    success = true,
                    message = "Code mapper entry added",
                    item = new
                    {
                        uuid = saved.UUID,
                        statusUUID = saved.StatusUUID,
                        statusTitle,
                        responseSchemaUUID = saved.ResponseSchemaUUID,
                        responseSchemaType = schemaType,
                        codeExampleUUID = saved.CodeExampleUUID,
                        codeExampleTitle = exampleTitle,
                        responseJson = saved.ResponseJson,
                        shortDescription = saved.ShortDescription
                    },
                    row
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    lists = new
                    {
                        status = vm.StatusList,
                        responseSchema = vm.ResponseSchemaList,
                        examples = vm.CodeExampleList
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCodeMapperByUuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return BadRequest(new { error = "uuid is required" });

            var dto = await _apiXCodeMapperService.GetByUuidAsync(uuid);
            if (dto == null) return NotFound(new { error = "not found" });

            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = dto.UUID,
                    apiXVersionUUID = dto.ApiXVersionUUID,
                    statusUUID = dto.StatusUUID,
                    codeExampleUUID = dto.CodeExampleUUID,
                    responseSchemaUUID = dto.ResponseSchemaUUID,
                    responseJson = dto.ResponseJson,
                    shortDescription = dto.ShortDescription
                }
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Header Status", MenuName = "ApiXHeader")]
        public Task<IActionResult> ToggleCodeMapper(string uuid)
        {
            // Reuse existing ToggleActiveAsync pattern used elsewhere in the controller
            return ToggleActiveAsync<ApiXCodeMapperDto, ApiXCodeMapperCommand>(uuid, _apiXCodeMapperService, "Code Mapper");
        }
        #endregion

       
        #region Language Snippet

        [HttpPost]
        public async Task<IActionResult> GetLanguageSnippets([FromForm] string? apiXVersionUUID)
        {
            var langs = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);
            var langDict = langs
                .Where(i => !string.IsNullOrWhiteSpace(i.Value))
                .ToDictionary(i => i.Value!.Trim(), i => i.Text ?? "");

            return await GetPagedByVersionAsync<ApiXLanguageContentDto, ApiXLanguageContentCommand>(
                _apiXLanguageContentService,
                apiXVersionUUID,
                dto => new Dictionary<string, object>
                {
                    ["uuid"] = dto.UUID,
                    ["languageTitle"] = (dto.LanguageUUID != null && langDict.TryGetValue(dto.LanguageUUID.Trim(), out var t) ? t : dto.LanguageUUID ?? ""),
                    ["languageContent"] = dto.LanguageContent ?? "",
                    ["action"] = GetToggleHtml(dto.UUID, dto.IsActive, nameof(ToggleLanguage), "Language Snippet")
                },
                // include inactive snippets so toggled rows remain visible
                async () => (IEnumerable<ApiXLanguageContentDto>)(await _apiXLanguageContentService.GetAllAsync()),
                dto => dto.ApiXVersionUUID
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLanguageSnippet([FromForm, Bind(Prefix = "LanguageSnippet")] ApiXLanguageContentCommand? cmd)
        {
            if (cmd == null) return BadRequest(new { error = "Invalid request" });
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return BadRequest(new
                {
                    success = false,
                    errors
                });
            }

            // Ensure ApiXVersionUUID is set (client sets hidden field before posting)
            if (string.IsNullOrWhiteSpace(cmd.ApiXVersionUUID))
            {
                var postedVersion = Request.Form["LanguageSnippet.ApiXVersionUUID"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(postedVersion)) cmd.ApiXVersionUUID = postedVersion;
            }

            try
            {
                if (await _apiXLanguageContentService.CheckDuplicateAsync(cmd))
                {
                    var fieldErrors = new Dictionary<string, string[]>
        {
            { "LanguageSnippet.LanguageUUID", new[] { "A snippet for this language already exists for this API version." } }
        };

                    return BadRequest(new
                    {
                        success = false,
                        errors = fieldErrors
                    });
                }
            }
            catch
            {
                // If duplicate-check fails, fall back to SaveAndReturnAsync which will still enforce server-side rules if implemented
            }
            try
            {
                var saved = await _apiXLanguageContentService.SaveAndReturnAsync(cmd, GetUserUUID(), Utils.GetLocalIPAddress());

                // Resolve friendly language title from master list (for DataTable display)
                var langs = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);
                var langTitle = langs.FirstOrDefault(i => string.Equals(i.Value?.Trim(), saved.LanguageUUID?.Trim(), StringComparison.OrdinalIgnoreCase))?.Text
                                ?? saved.LanguageUUID ?? "";

                var row = new Dictionary<string, object>
                {
                    ["uuid"] = saved.UUID,
                    ["languageTitle"] = langTitle,
                    ["languageContent"] = saved.LanguageContent ?? "",
                    ["isActive"] = saved.IsActive,
                    // keep edit anchor so client-side `.edit-snippet` handlers work
                    ["action"] = "<a href=\"javascript:void(0);\" class=\"edit-snippet\">Edit</a>"
                };

                return Ok(new
                {
                    success = true,
                    message = "Snippet saved",
                    // include apiXVersionUUID and isActive in item so client can immediately populate the form
                    item = new
                    {
                        uuid = saved.UUID,
                        languageUUID = saved.LanguageUUID,
                        languageTitle = langTitle,
                        languageContent = saved.LanguageContent,
                        apiXVersionUUID = saved.ApiXVersionUUID,
                        isActive = saved.IsActive
                    },
                    row
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLanguageSnippetByUuid(string? uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid)) return BadRequest(new { error = "uuid is required" });

            var dto = await _apiXLanguageContentService.GetByUuidAsync(uuid);
            if (dto == null) return NotFound(new { error = "not found" });

            // Resolve friendly language title
            var langs = await LoadDropdownAsync(_masterProgrammingLanguageService, x => x.Title);
            var langTitle = langs.FirstOrDefault(i => string.Equals(i.Value?.Trim(), dto.LanguageUUID?.Trim(), StringComparison.OrdinalIgnoreCase))?.Text
                            ?? dto.LanguageUUID ?? "";

            return Ok(new
            {
                success = true,
                item = new
                {
                    uuid = dto.UUID,
                    apiXVersionUUID = dto.ApiXVersionUUID,
                    languageUUID = dto.LanguageUUID,
                    languageTitle = langTitle,
                    languageContent = dto.LanguageContent,
                    isActive = dto.IsActive// return actual snippet content
                }
            });
        }

        [HttpPost]
        [ActivityLog(ActivityType = "Update", Description = "Toggled APIX Header Status", MenuName = "ApiXHeader")]
        public Task<IActionResult> ToggleLanguage(string uuid)
        {
            // Reuse existing ToggleActiveAsync pattern used elsewhere in the controller
            return ToggleActiveAsync<ApiXLanguageContentDto, ApiXLanguageContentCommand>(uuid, _apiXLanguageContentService, "Language Snippet");
        }

       

        #endregion
    }
}

