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
    public class ApixHeaderService : MasterServiceBase<ApiXHeaders, ApixHeaderDto, ApixHeaderCommand>, IApixHeaderService
    {
        public ApixHeaderService(IMasterRepository<ApiXHeaders> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
        public async Task<List<ApixHeaderDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApixHeaderDto>>(entities);
        }

        public async Task<List<ApixHeaderDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x => true);
            return _mapper.Map<List<ApixHeaderDto>>(entities);
        }
        // Prevent duplicate field names for same ApiX version (except when updating the same record)
        protected override async Task<bool> IsDuplicateAsync(ApixHeaderCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var field = (command.FieldName ?? string.Empty).ToLower().Trim();
            var versionUuid = command.ApiXVersionUUID ?? string.Empty;

            return await _repository.ExistsAsync(x =>
                x.ApiXVersionUUID == versionUuid &&
                (x.FieldName ?? "").ToLower().Trim() == field &&
                x.UUID != command.UUID);
        }
        public Task<bool> CheckDuplicateAsync(ApixHeaderCommand command)
        {
            return IsDuplicateAsync(command);
        }
        // Search used by MasterServiceBase paged queries
        protected override Expression<Func<ApiXHeaders, bool>>? BuildSearchFilter(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return null;
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.FieldName != null && x.FieldName.ToLower().Contains(searchTerm)) ||
                (x.FieldDetails != null && x.FieldDetails.ToLower().Contains(searchTerm));
        }

        // Default sort for datatables / paged queries
        protected override Func<IQueryable<ApiXHeaders>, IOrderedQueryable<ApiXHeaders>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "fieldname" => q => isAsc ? q.OrderBy(x => x.FieldName) : q.OrderByDescending(x => x.FieldName),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<List<ApixHeaderDto>> GetByVersionAsync(string versionUuid)
        {
            var entities = await _repository.GetAllActiveAsync();

            var filtered = entities
                .Where(x => x.ApiXVersionUUID == versionUuid)
                .ToList();

            return _mapper.Map<List<ApixHeaderDto>>(filtered);
        }

        // New: save and return the saved DTO (create or update)
        public async Task<ApixHeaderDto> SaveAndReturnAsync(ApixHeaderCommand command, string userUuid, string ip)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            // Run validation and duplicate checks consistent with MasterServiceBase.SaveAsync
            await ValidateAsync(command);

            if (await IsDuplicateAsync(command))
            {
                throw new Exception($"{EntityDisplayName} already exists.");
            }

            if (string.IsNullOrEmpty(command.UUID))
            {
                // Create
                var entity = _mapper.Map<ApiXHeaders>(command);
                entity.UUID = Utils.GetUUID();
                entity.IsActive = true;

                OnBeforeCreate(entity, userUuid, ip);

                await _repository.AddAsync(entity);

                return _mapper.Map<ApixHeaderDto>(entity);
            }
            else
            {
                // Update
                var entity = await _repository.GetByUuidAsync(command.UUID) ?? throw new Exception("Record not found");
                var existingIsActive = entity.IsActive;
                _mapper.Map(command, entity);
                // restore preserved IsActive (use toggle endpoint to change IsActive)
                entity.IsActive = existingIsActive;
                OnBeforeUpdate(entity, userUuid, ip);

                await _repository.UpdateAsync(entity);

                return _mapper.Map<ApixHeaderDto>(entity);
            }
        }
    }
}