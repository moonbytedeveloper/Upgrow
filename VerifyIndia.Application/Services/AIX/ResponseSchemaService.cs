using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.AIX;
using VerifyIndia.Application.DTO.AIX;
using VerifyIndia.Application.IServices.AIX;
using VerifyIndia.Application.Services.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.AIX
{
    public class ResponseSchemaService : MasterServiceBase<ResponseSchema, ResponseSchemaDto, ResponseSchemaCommand>, IResponseSchemaService
    {
        private readonly IMasterRepository<ReqResSchemaFields> _schemaFieldRepository;
        public ResponseSchemaService(IMasterRepository<ResponseSchema> repository, IMapper mapper, IMasterRepository<ReqResSchemaFields> schemaFieldRepository)
          : base(repository, mapper) 
        {
            _schemaFieldRepository = schemaFieldRepository;
        }

        public async Task<List<ResponseSchemaDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ResponseSchemaDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ResponseSchemaCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ApiXVersionUUID!.ToLower().Trim() == command.ApiXVersionUUID!.ToLower().Trim()
                && x.ShortDescription == command.ShortDescription
                && x.UUID != command.UUID);
        }
        public async Task<ResponseSchemaDto> SaveAndReturnAsync(ResponseSchemaCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.IsActive = true;
            // Save (MasterServiceBase implements SaveAsync)
            await SaveAsync(command, userUuid, ip);

            if (string.IsNullOrWhiteSpace(command.UUID))
                throw new InvalidOperationException("Save failed to produce UUID.");

            var dto = await GetByUuidAsync(command.UUID);
            if (dto == null)
                throw new InvalidOperationException("Failed to retrieve saved RequestSchema.");

            var responseDto = _mapper.Map<ResponseSchemaDto>(dto);
            return responseDto;
        }

        // Define which fields to search
        protected override Expression<Func<ResponseSchema, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiXVersionUUID != null && x.ApiXVersionUUID.ToLower().Contains(searchTerm)) ||
                (x.SchemaType != null && x.SchemaType.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<ResponseSchema>, IOrderedQueryable<ResponseSchema>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "provider" => q => isAsc ? q.OrderBy(x => x.ApiXVersionUUID) : q.OrderByDescending(x => x.ApiXVersionUUID),
                "api" => q => isAsc ? q.OrderBy(x => x.SchemaType) : q.OrderByDescending(x => x.SchemaType),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<ResponseSchema?> GetByUuidAsync(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return null;

                var schema = await _repository.GetByUuidAsync(uuid);
                if (schema != null && !schema.IsActive)
                    return null;

                return schema;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching response schema: {ex.Message}", ex);
            }
        }

        public async Task<List<ReqResSchemaFields>> GetFieldsBySchemaUuidAsync(string schemaUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schemaUuid))
                    return new List<ReqResSchemaFields>();

                var fields = await _schemaFieldRepository.FindAllAsync(
                    x => x.SchemaUUID == schemaUuid && x.IsActive);

                return fields.OrderBy(f => f.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching schema fields: {ex.Message}", ex);
            }
        }

        public async Task<ResponseSchemaSectionDto?> GetResponseSchemaBySectionAsync(string responseSchemaUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(responseSchemaUuid))
                    return null;

                var schema = await GetByUuidAsync(responseSchemaUuid);
                if (schema == null)
                    return null;

                var allFields = await GetFieldsBySchemaUuidAsync(responseSchemaUuid);

                var responseSchemaSectionDto = new ResponseSchemaSectionDto
                {
                    SchemaUUID = schema.UUID,
                    ShortDescription = schema.ShortDescription,
                    SchemaType = schema.SchemaType,
                    Fields = BuildHierarchy(allFields)
                };

                return responseSchemaSectionDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching response schema section: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Builds parent-child hierarchy from flat list of fields
        /// </summary>
        private List<ResponseSchemaFieldDto> BuildHierarchy(List<ReqResSchemaFields> allFields)
        {
            // Get all root-level fields (no parent)
            var rootFields = allFields
                .Where(f => string.IsNullOrWhiteSpace(f.ParentUUID))
                .OrderBy(f => f.DisplayOrder)
                .ToList();

            var result = new List<ResponseSchemaFieldDto>();
            foreach (var rootField in rootFields)
            {
                result.Add(MapToDtoRecursive(rootField, allFields));
            }

            return result;
        }

        private ResponseSchemaFieldDto MapToDtoRecursive(ReqResSchemaFields field, List<ReqResSchemaFields> allFields)
        {
            // Find children for this field
            var children = allFields
                .Where(f => f.ParentUUID == field.UUID)
                .OrderBy(f => f.DisplayOrder)
                .ToList();

            var isParentNode = children.Any();
            var dto = MapFieldToDto(field, isParentNode);

            if (isParentNode)
            {
                dto.ChildFields = children.Select(c => MapToDtoRecursive(c, allFields)).ToList();
            }

            return dto;
        }

        /// <summary>
        /// Maps ReqResSchemaFields entity to ResponseSchemaFieldDto
        /// </summary>
        private ResponseSchemaFieldDto MapFieldToDto(ReqResSchemaFields field, bool isParentNode = false)
        {
            return new ResponseSchemaFieldDto
            {
                UUID = field.UUID,
                FieldName = field.FieldName,
                DataType = field.DataType,
                //Logic preserved: Nullify for parents
                Description = !isParentNode ? field.Description : null,
                Examlpe = !isParentNode ? field.Examlpe : null,
                Constraints = field.Constraints,
                AllowNull = field.AllowNull,
                ParentUUID = field.ParentUUID,
                DisplayOrder = field.DisplayOrder,
                IsArray = field.IsArray,
                ChildFields = new List<ResponseSchemaFieldDto>()
            };
        }



    }
}

