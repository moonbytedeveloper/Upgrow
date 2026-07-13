/*using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiDashboardService : IApiDashboardService
    {
        private readonly IApiXCategoryService _apiXCategoryService;
        private readonly IApiXVersionService _apiXVersionService;
        private readonly IApiXHeaderService _apiXHeaderService;
        private readonly IRequestSchemaService _requestSchemaService;
        private readonly IApiXCodeMapperService _codeMapperService;
        private readonly IApiXStatusCodeService _statusCodeService;
        private readonly IApiXCodeExampleService _codeExampleService;
        private readonly IResponseSchemaService _responseSchemaService;

        public ApiDashboardService(
            IApiXCategoryService apiXCategoryService,
            IApiXVersionService apiXVersionService,
            IApiXHeaderService apiXHeaderService,
            IRequestSchemaService requestSchemaService,
            IApiXCodeMapperService codeMapperService,
            IApiXStatusCodeService statusCodeService,
            IApiXCodeExampleService codeExampleService,
            IResponseSchemaService responseSchemaService)
        {
            _apiXCategoryService = apiXCategoryService;
            _apiXVersionService = apiXVersionService;
            _apiXHeaderService = apiXHeaderService;
            _requestSchemaService = requestSchemaService;
            _codeMapperService = codeMapperService;
            _statusCodeService = statusCodeService;
            _codeExampleService = codeExampleService;
            _responseSchemaService = responseSchemaService;
        }

        public async Task<DashboardContentDto> GetDashboardContentAsync(string xCategoryUuid, string versionUuid)
        {
            try
            {
                var content = new DashboardContentDto();

                // Fetch API X Category
                var xCategory = await _apiXCategoryService.GetByUuidAsync(xCategoryUuid);
                if (xCategory == null)
                    throw new InvalidOperationException($"API Category with UUID {xCategoryUuid} not found.");

                // Fetch API X Version
                var version = await _apiXVersionService.GetByUuidAsync(versionUuid);
                if (version == null)
                    throw new InvalidOperationException($"API Version with UUID {versionUuid} not found.");

                // Section 1: Header Info
                content.HeaderInfo = new HeaderInfoSectionDto
                {
                    ApiTitle = xCategory.Title,
                    ApiVersion = version.VersionNo,
                    ApiDescription = xCategory.Description,
                    BreadcrumbCategory = xCategory.Title
                };

                // Section 2: Operation Details
                content.OperationDetails = new OperationDetailsSectionDto
                {
                    HttpMethod = version.ApiMethod?.ToUpper() ?? "POST",
                    ApiPath = version.ApiPath?.TrimStart('/') ?? string.Empty,
                    OperationTitle = version.Title,
                    OperationDescription = version.Description,
                    IsRequestBodyRequired = version.IsRequestBodyRequired
                };

                // Section 3: Headers
                var headers = await _apiXHeaderService.GetByApiXVersionUUIDAsync(versionUuid);
                content.Headers = headers.Select(h => new HeaderTableSectionDto
                {
                    FieldName = h.FieldName,
                    FieldDetails = h.FieldDetails,
                    DataType = h.DataType,
                    IsRequired = h.IsRequired
                }).ToList();

                 
                content.RequestSchema = await GetRequestSchemaAsync(versionUuid);
                content.ResponseInspector = await GetResponseInspectorAsync(versionUuid);


                return content;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching dashboard content: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get request schema using RequestSchemaService
        /// </summary>
        private async Task<RequestSchemaSectionDto> GetRequestSchemaAsync(string versionUuid)
        {
            var requestSchemaDto = new RequestSchemaSectionDto();

            try
            {
                 
                var (schema, fields) = await _requestSchemaService.GetCompleteSchemaAsync(versionUuid);

                if (schema == null)
                    return requestSchemaDto;

                requestSchemaDto.RequestType = schema.RequestType ?? " ";
                requestSchemaDto.RequestJson = schema.RequestJson;  
                requestSchemaDto.Fields = fields.Select(f => new RequestSchemaFieldDto
                {
                    FieldName = f.FieldName,
                    DataType = f.DataType,
                    Description = f.Description,
                    Examlpe = f.Examlpe,
                    Constraints = f.Constraints,
                    AllowNull = f.AllowNull
                }).ToList();

                return requestSchemaDto;
            }
            catch (Exception ex)
            {
                // Return empty schema on error
                Console.WriteLine($"Error fetching request schema: {ex.Message}");
                return requestSchemaDto;
            }
        }

        private async Task<ResponseInspectorDto> GetResponseInspectorAsync(string versionUuid)
        {
            var responseInspector = new ResponseInspectorDto();

            try
            {
                // Fetch all code mappers for this version
                var codeMappers = await _codeMapperService.GetByVersionUuidAsync(versionUuid);

                if (!codeMappers.Any())
                    return responseInspector;

                // Get unique status UUIDs
                var statusUuids = codeMappers
                    .Where(m => !string.IsNullOrWhiteSpace(m.StatusUUID))
                    .Select(m => m.StatusUUID!)
                    .Distinct()
                    .ToList();

                // Fetch all status codes for these UUIDs
                var statusCodes = await _statusCodeService.GetByUuidsAsync(statusUuids);

                // Build response with statuses and their examples
                foreach (var statusCode in statusCodes)
                {
                    var statusWithExamples = new ResponseStatusWithExamplesDto
                    {
                        Status = statusCode,
                        Examples = new List<ResponseExampleWithMapperDto>()
                    };

                    // Get mappers for this status
                    var mappersForStatus = codeMappers
                        .Where(m => m.StatusUUID == statusCode.UUID)
                        .ToList();

                    // For each mapper, get the example and add response JSON
                    foreach (var mapper in mappersForStatus)
                    {
                        if (!string.IsNullOrWhiteSpace(mapper.CodeExampleUUID))
                        {
                            var example = await _codeExampleService.GetByUuidAsync(mapper.CodeExampleUUID);
                            if (example != null)
                            {
                                statusWithExamples.Examples!.Add(new ResponseExampleWithMapperDto
                                {
                                    ExampleUUID = example.UUID,
                                    ErrorTitle = example.ErrorTitle,
                                    Message = example.Message,
                                    ResponseJson = mapper.ResponseJson
                                });
                            }
                        }
                    }

                    if (statusWithExamples.Examples!.Any())
                    {
                        responseInspector.StatusesWithExamples!.Add(statusWithExamples);
                    }
                }

                return responseInspector;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching response inspector data: {ex.Message}");
                return responseInspector;
            }
        }




    }
}*/

using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.AIX;
using VerifyIndia.Application.IServices.Api;
using VerifyIndia.Application.IServices.Master;

namespace VerifyIndia.Application.Services.Api
{
    public class ApiDashboardService : IApiDashboardService
    {
        private readonly IApiXCategoryService _apiXCategoryService;
        private readonly IApiXVersionService _apiXVersionService;
        private readonly IApiXHeaderService _apiXHeaderService;
        private readonly IRequestSchemaService _requestSchemaService;
        private readonly IApiXCodeMapperService _codeMapperService;
        private readonly IApiXStatusCodeService _statusCodeService;
        private readonly IApiXCodeExampleService _codeExampleService;
        private readonly IResponseSchemaService _responseSchemaService;
        private readonly IMaster_ProgrammingLanguageService _programmingLanguageService;
        private readonly IApiXLanguageContentService _languageContentService;

        public ApiDashboardService(
            IApiXCategoryService apiXCategoryService,
            IApiXVersionService apiXVersionService,
            IApiXHeaderService apiXHeaderService,
            IRequestSchemaService requestSchemaService,
            IApiXCodeMapperService codeMapperService,
            IApiXStatusCodeService statusCodeService,
            IApiXCodeExampleService codeExampleService,
            IResponseSchemaService responseSchemaService,
            IApiXLanguageContentService languageContentService,
            IMaster_ProgrammingLanguageService programmingLanguageService
            )
        {
            _apiXCategoryService = apiXCategoryService;
            _apiXVersionService = apiXVersionService;
            _apiXHeaderService = apiXHeaderService;
            _requestSchemaService = requestSchemaService;
            _codeMapperService = codeMapperService;
            _statusCodeService = statusCodeService;
            _codeExampleService = codeExampleService;
            _responseSchemaService = responseSchemaService;
            _languageContentService = languageContentService;
            _programmingLanguageService = programmingLanguageService;
        }

        public async Task<DashboardContentDto> GetDashboardContentAsync(string xCategoryUuid, string versionUuid)
        {
            try
            {
                var content = new DashboardContentDto();

                // Fetch API X Category
                var xCategory = await _apiXCategoryService.GetByUuidAsync(xCategoryUuid);
                if (xCategory == null)
                    throw new InvalidOperationException($"API Category with UUID {xCategoryUuid} not found.");

                // Fetch API X Version
                var version = await _apiXVersionService.GetByUuidAsync(versionUuid);
                if (version == null)
                    throw new InvalidOperationException($"API Version with UUID {versionUuid} not found.");

                // Section 1: Header Info
                content.HeaderInfo = new HeaderInfoSectionDto
                {
                    ApiTitle = xCategory.Title,
                    ApiVersion = version.VersionNo,
                    ApiDescription = xCategory.Description,
                    BreadcrumbCategory = xCategory.Title
                };

                // Section 2: Operation Details
                content.OperationDetails = new OperationDetailsSectionDto
                {
                    HttpMethod = version.ApiMethod?.ToUpper() ?? "POST",
                    ApiPath = version.ApiPath?.TrimStart('/') ?? string.Empty,
                    OperationTitle = version.Title,
                    OperationDescription = version.Description,
                    IsRequestBodyRequired = version.IsRequestBodyRequired
                };

                // Section 3: Headers
                var headers = await _apiXHeaderService.GetByApiXVersionUUIDAsync(versionUuid);
                content.Headers = headers.Select(h => new HeaderTableSectionDto
                {
                    FieldName = h.FieldName,
                    FieldDetails = h.FieldDetails,
                    DataType = h.DataType,
                    IsRequired = h.IsRequired
                }).ToList();

                // Section 4: Request Schema
                content.RequestSchema = await GetRequestSchemaAsync(versionUuid);

                // Section 5: Response Inspector (with Response Schema)
                content.ResponseInspector = await GetResponseInspectorAsync(versionUuid);

                // Section 6: Language Content
                var languageContent = await GetLanguageContentAsync(versionUuid);
                content.LanguageContent = languageContent;

                return content;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching dashboard content: {ex.Message}", ex);
            }
        }

        private async Task<RequestSchemaSectionDto> GetRequestSchemaAsync(string versionUuid)
        {
            var requestSchemaDto = new RequestSchemaSectionDto();

            try
            {
                var (schema, fields) = await _requestSchemaService.GetCompleteSchemaAsync(versionUuid);

                if (schema == null)
                    return requestSchemaDto;

                requestSchemaDto.RequestType = schema.RequestType ?? " ";
                requestSchemaDto.RequestJson = schema.RequestJson;
                requestSchemaDto.Fields = fields.Select(f => new RequestSchemaFieldDto
                {
                    FieldName = f.FieldName,
                    DataType = f.DataType,
                    Description = f.Description,
                    Examlpe = f.Examlpe,
                    Constraints = f.Constraints,
                    AllowNull = f.AllowNull
                }).ToList();

                return requestSchemaDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching request schema: {ex.Message}");
                return requestSchemaDto;
            }
        }

        private async Task<ResponseInspectorDto> GetResponseInspectorAsync(string versionUuid)
        {
            var responseInspector = new ResponseInspectorDto();

            try
            {
                // Fetch all code mappers for this version
                var codeMappers = await _codeMapperService.GetByVersionUuidAsync(versionUuid);

                if (!codeMappers.Any())
                    return responseInspector;

                // Get unique status UUIDs
                var statusUuids = codeMappers
                    .Where(m => !string.IsNullOrWhiteSpace(m.StatusUUID))
                    .Select(m => m.StatusUUID!)
                    .Distinct()
                    .ToList();

                // Fetch all status codes for these UUIDs
                var statusCodes = await _statusCodeService.GetByUuidsAsync(statusUuids);

                // Build response with statuses and their examples
                foreach (var statusCode in statusCodes)
                {
                    var statusWithExamples = new ResponseStatusWithExamplesDto
                    {
                        Status = statusCode,
                        Examples = new List<ResponseExampleWithMapperDto>()
                    };

                    // Get mappers for this status
                    var mappersForStatus = codeMappers
                        .Where(m => m.StatusUUID == statusCode.UUID)
                        .ToList();

                    if (mappersForStatus.Any())
                    {
                        var firstMapper = mappersForStatus.FirstOrDefault();
                        if (firstMapper != null && !string.IsNullOrWhiteSpace(firstMapper.ResponseSchemaUUID))
                        {
                            statusWithExamples.ResponseSchema = await _responseSchemaService
                                .GetResponseSchemaBySectionAsync(firstMapper.ResponseSchemaUUID);
                        }
                    }

                    

                    // Add all examples for this status
                    foreach (var mapper in mappersForStatus)
                    {
                        if (!string.IsNullOrWhiteSpace(mapper.CodeExampleUUID))
                        {
                            var example = await _codeExampleService.GetByUuidAsync(mapper.CodeExampleUUID);
                            if (example != null)
                            {
                                statusWithExamples.Examples!.Add(new ResponseExampleWithMapperDto
                                {
                                    ExampleUUID = example.UUID,
                                    ErrorTitle = example.ErrorTitle,
                                    Message = example.Message,
                                    ResponseJson = mapper.ResponseJson
                                });
                            }
                        }
                    }

                    if (statusWithExamples.Examples!.Any())
                    {
                        responseInspector.StatusesWithExamples!.Add(statusWithExamples);
                    }
                }

                return responseInspector;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching response inspector data: {ex.Message}");
                return responseInspector;
            }
        }

        private async Task<LanguageContentSectionDto> GetLanguageContentAsync(string versionUuid)
        {
            var languageContentDto = new LanguageContentSectionDto();

            try
            {
                // Get all active programming languages
                languageContentDto.AvailableLanguages = await _programmingLanguageService.GetAllActiveLangunagesAsync();

                if (languageContentDto.AvailableLanguages.Any())
                {
                    // Get content for the first language by default
                    var firstLanguage = languageContentDto.AvailableLanguages.First();
                    languageContentDto.SelectedLanguageContent = await _languageContentService
                        .GetByVersionAndLanguageAsync(versionUuid, firstLanguage.UUID);
                }

                return languageContentDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching language content: {ex.Message}");
                return languageContentDto;
            }
        }
    }
}