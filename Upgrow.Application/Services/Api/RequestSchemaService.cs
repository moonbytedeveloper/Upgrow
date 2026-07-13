using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.AIX;
using Upgrow.Application.Commands.Api;
using Upgrow.Application.DTO.AIX;
using Upgrow.Application.DTO.Api;
using Upgrow.Application.IServices.AIX;
using Upgrow.Application.IServices.Api;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Api
{
    public class RequestSchemaService : MasterServiceBase<RequestSchema, RequestSchemaDto, RequestSchemaCommand>, IRequestSchemaService
    {
        private readonly IMasterRepository<RequestSchema> _requestSchemaRepository;
        private readonly IMasterRepository<ReqResSchemaFields> _schemaFieldRepository;
        private readonly IMapper _mapper;

        public RequestSchemaService(
            IMasterRepository<RequestSchema> repository,
            IMasterRepository<ReqResSchemaFields> schemaFieldRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _requestSchemaRepository = repository;
            _schemaFieldRepository = schemaFieldRepository;
            _mapper = mapper;
        }

        public async Task<RequestSchemaDto?> GetByVersionUuidAsync(string versionUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(versionUuid))
                    return null;

                var schemas = await _requestSchemaRepository.FindAllAsync(
                    x => x.ApiXVersionUUID == versionUuid && x.IsActive);

                var schema = schemas.FirstOrDefault();
                return schema == null ? null : _mapper.Map<RequestSchemaDto>(schema);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching request schema by version UUID: {ex.Message}", ex);
            }
        }

        public async Task<List<ReqResSchemaFieldDto>> GetFieldsBySchemaUuidAsync(string schemaUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(schemaUuid))
                    return new List<ReqResSchemaFieldDto>();

                var fields = await _schemaFieldRepository.FindAllAsync(
                    x => x.SchemaUUID == schemaUuid && x.IsActive);

                return _mapper.Map<List<ReqResSchemaFieldDto>>(
                    fields.OrderBy(f => f.DisplayOrder).ToList());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching schema fields: {ex.Message}", ex);
            }
        }

        public async Task<(RequestSchemaDto? Schema, List<ReqResSchemaFieldDto> Fields)> GetCompleteSchemaAsync(string versionUuid)
        {
            try
            {
                var schema = await GetByVersionUuidAsync(versionUuid);
                if (schema == null)
                    return (null, new List<ReqResSchemaFieldDto>());

                var fields = await GetFieldsBySchemaUuidAsync(schema.UUID!);
                return (schema, fields);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching complete schema: {ex.Message}", ex);
            }
        }

        public async Task<List<RequestSchemaDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<RequestSchemaDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(RequestSchemaCommand command)
        {
            return false;
        }
        public async Task<RequestSchemaDto> SaveAndReturnAsync(
    RequestSchemaCommand command,
    string userUuid,
    string ip)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!string.IsNullOrWhiteSpace(command.UUID))
            {
                // manual safe update (bypass base UpdateAsync behavior)
                var entity = await _repository.GetByUuidAsync(command.UUID)
                    ?? throw new Exception("Record not found");

                var isActive = entity.IsActive;

                _mapper.Map(command, entity);

                entity.IsActive = isActive;

                OnBeforeUpdate(entity, userUuid, ip);

                await _repository.UpdateAsync(entity);

                command.UUID = entity.UUID;
            }
            else
            {
                await SaveAsync(command, userUuid, ip);
            }

            var dto = await GetByUuidAsync(command.UUID!);
            return dto!;
        }

        // Define which fields to search
        protected override Expression<Func<RequestSchema, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiXVersionUUID != null && x.ApiXVersionUUID.ToLower().Contains(searchTerm)) ||
                (x.RequestJson != null && x.RequestType.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<RequestSchema>, IOrderedQueryable<RequestSchema>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "provider" => q => isAsc ? q.OrderBy(x => x.ApiXVersionUUID) : q.OrderByDescending(x => x.ApiXVersionUUID),
                "api" => q => isAsc ? q.OrderBy(x => x.RequestType) : q.OrderByDescending(x => x.RequestType),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }


    }
}