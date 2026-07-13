using System;
using System.Collections.Generic;

namespace Upgrow.Application.DTO.AIX
{
   
    public class HeaderInfoSectionDto
    {
        public string? ApiTitle { get; set; }
        public string? ApiVersion { get; set; }
        public string? ApiDescription { get; set; }
        public string? BreadcrumbCategory { get; set; }
 
    }

     
    public class OperationDetailsSectionDto
    {
        public string? HttpMethod { get; set; }
        public string? ApiPath { get; set; }
        public string? OperationTitle { get; set; }
        public string? OperationDescription { get; set; }
        public bool IsRequestBodyRequired { get; set; }
    }

 
    public class HeaderTableSectionDto
    {
        public string? FieldName { get; set; }
        public string? FieldDetails { get; set; }
        public string? DataType { get; set; }
        public bool IsRequired { get; set; }
    }

    public class RequestSchemaFieldDto
    {
        public string? FieldName { get; set; }
        public string? DataType { get; set; }
        public string? Description { get; set; }
        public string? Examlpe { get; set; }
        public string? Constraints { get; set; }
        public bool AllowNull { get; set; }
    }

    public class RequestSchemaSectionDto
    {
        public string? RequestType { get; set; }
        public string? RequestJson { get; set; } // ? Direct JSON from database
        public List<RequestSchemaFieldDto>? Fields { get; set; }

        public RequestSchemaSectionDto()
        {
            Fields = new List<RequestSchemaFieldDto>();
        }
    }

    public class ResponseSchemaFieldDto
    {
        public string? UUID { get; set; }
        public string? FieldName { get; set; }
        public string? DataType { get; set; }
        public string? Description { get; set; }
        public string? Examlpe { get; set; }
        public string? Constraints { get; set; }
        public bool AllowNull { get; set; }
        public string? ParentUUID { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsArray { get; set; }
        public List<ResponseSchemaFieldDto>? ChildFields { get; set; }

        public ResponseSchemaFieldDto()
        {
            ChildFields = new List<ResponseSchemaFieldDto>();
        }
    }

    // NEW: Response Schema Section DTO
    public class ResponseSchemaSectionDto
    {
        public string? SchemaUUID { get; set; }
        public string? ShortDescription { get; set; }
        public string? SchemaType { get; set; }
        public List<ResponseSchemaFieldDto>? Fields { get; set; }

        public ResponseSchemaSectionDto()
        {
            Fields = new List<ResponseSchemaFieldDto>();
        }
    }

    public class ResponseStatusWithExamplesDto
    {
        public ResponseStatusDto? Status { get; set; }
        public ResponseSchemaSectionDto? ResponseSchema { get; set; }
        public List<ResponseExampleWithMapperDto>? Examples { get; set; }

        public ResponseStatusWithExamplesDto()
        {
            Examples = new List<ResponseExampleWithMapperDto>();
        }
    }

 
    public class ResponseExampleWithMapperDto
    {
        public string? ExampleUUID { get; set; }
        public string? ErrorTitle { get; set; }
        public string? Message { get; set; }
        public string? ResponseJson { get; set; }
    }

 
    public class ResponseInspectorDto
    {
        public List<ResponseStatusWithExamplesDto>? StatusesWithExamples { get; set; }

        public ResponseInspectorDto()
        {
            StatusesWithExamples = new List<ResponseStatusWithExamplesDto>();
        }
    }


    public class DashboardContentDto
    {
        public HeaderInfoSectionDto? HeaderInfo { get; set; }
        public OperationDetailsSectionDto? OperationDetails { get; set; }
        public List<HeaderTableSectionDto>? Headers { get; set; }

        public RequestSchemaSectionDto? RequestSchema { get; set; }

        public ResponseInspectorDto? ResponseInspector { get; set; }
        public LanguageContentSectionDto? LanguageContent { get; internal set; }
        public object? BaseUrl { get; set; }

        public DashboardContentDto()
        {
            Headers = new List<HeaderTableSectionDto>();
            RequestSchema = new RequestSchemaSectionDto();
            ResponseInspector = new ResponseInspectorDto();

        }
    }
}