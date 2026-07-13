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
    public class ApiXCodeMapperService : MasterServiceBase<ApiXCodeMapper, ApiXCodeMapperDto, ApiXCodeMapperCommand>, IApiXCodeMapperService
    {
        public ApiXCodeMapperService(IMasterRepository<ApiXCodeMapper> repository, IMapper mapper)
         : base(repository, mapper) { }

        public async Task<List<ApiXCodeMapperDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXCodeMapperDto>>(entities);
        }
        public async Task<List<ApiXCodeMapperDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x => true);
            return _mapper.Map<List<ApiXCodeMapperDto>>(entities);
        }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(ApiXCodeMapperCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.ResponseSchemaUUID!.ToLower().Trim() == command.ResponseSchemaUUID.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        public Task<bool> CheckDuplicateAsync(ApiXCodeMapperCommand command)
        {
            return IsDuplicateAsync(command);
        }
        public async Task<ApiXCodeMapperDto> SaveAndReturnAsync(ApiXCodeMapperCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            // run any validation logic defined in the base class
            await ValidateAsync(command);

            // Create
            if (string.IsNullOrEmpty(command.UUID))
            {
                var entity = _mapper.Map<ApiXCodeMapper>(command);
                entity.UUID = Utils.GetUUID();
                entity.IsActive = command.IsActive;

                OnBeforeCreate(entity, userUuid, ip);

                await _repository.AddAsync(entity);

                return _mapper.Map<ApiXCodeMapperDto>(entity);
            }
            else
            {
                // Update
                var entity = await _repository.GetByUuidAsync(command.UUID) ?? throw new Exception("Record not found");
                _mapper.Map(command, entity);

                OnBeforeUpdate(entity, userUuid, ip);

                await _repository.UpdateAsync(entity);

                return _mapper.Map<ApiXCodeMapperDto>(entity);
            }
        }

        // Define which fields to search
        protected override Expression<Func<ApiXCodeMapper, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiXVersionUUID != null && x.ApiXVersionUUID.ToLower().Contains(searchTerm)) ||
                (x.ResponseSchemaUUID != null && x.ResponseSchemaUUID.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<ApiXCodeMapper>, IOrderedQueryable<ApiXCodeMapper>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "provider" => q => isAsc ? q.OrderBy(x => x.ApiXVersionUUID) : q.OrderByDescending(x => x.ApiXVersionUUID),
                "api" => q => isAsc ? q.OrderBy(x => x.ResponseSchemaUUID) : q.OrderByDescending(x => x.ResponseSchemaUUID),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<ApiXCodeMapperDto?> GetByUuidAsync(string uuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(uuid))
                    return null;

                var mapper = await _repository.GetByUuidAsync(uuid);
                return mapper == null ? null : _mapper.Map<ApiXCodeMapperDto>(mapper);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching code mapper by UUID: {ex.Message}", ex);
            }
        }

        public async Task<List<ApiXCodeMapperDto>> GetByVersionUuidAsync(string versionUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(versionUuid))
                    return new List<ApiXCodeMapperDto>();

                var mappers = await _repository.FindAllAsync(
                    x => x.ApiXVersionUUID == versionUuid && x.IsActive);

                return _mapper.Map<List<ApiXCodeMapperDto>>(mappers);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching code mappers by version UUID: {ex.Message}", ex);
            }
        }

        public async Task<List<ApiXCodeMapperDto>> GetByStatusUuidAsync(string statusUuid)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(statusUuid))
                    return new List<ApiXCodeMapperDto>();

                var mappers = await _repository.FindAllAsync(
                    x => x.StatusUUID == statusUuid && x.IsActive);

                return _mapper.Map<List<ApiXCodeMapperDto>>(mappers);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching code mappers by status UUID: {ex.Message}", ex);
            }
        }

    }
}


