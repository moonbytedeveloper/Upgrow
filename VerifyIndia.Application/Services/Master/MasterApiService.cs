using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterApiService : MasterServiceBase<Master_Api, MasterApiDto, MasterApiCommand>, IMasterApiService
    {
        public MasterApiService(IMasterRepository<Master_Api> repository, IMapper mapper)
            : base(repository, mapper) { }

        // Generate code from API name: uppercase with underscores instead of spaces
        private string GenerateCode(string apiName)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                apiName.ToUpper().Trim(),
                @"[^A-Z0-9]+",
                "_").TrimEnd('_');
        }

        public async Task<MasterApiDto> SaveAsync(
    MasterApiCommand command,
    string userUUID,
    string ipAddress,
    bool saveChanges = true)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            await ValidateAsync(command);

            if (await IsDuplicateAsync(command))
                throw new Exception($"{EntityDisplayName} already exists.");

            Master_Api entity;

            if (string.IsNullOrEmpty(command.UUID))
            {
                // INSERT
                entity = _mapper.Map<Master_Api>(command); 

                entity.UUID = Utils.GetUUID();
                entity.Code = GenerateCode(entity.ApiName);
                entity.IsActive = true;

                OnBeforeCreate(entity, userUUID, ipAddress);

                await _repository.AddAsync(entity);
                if (saveChanges)
                    {
                    await _repository.SaveChangesAsync();
                }
            }
            else
            {
                // UPDATE
                entity = await _repository.GetByUuidAsync(command.UUID)
                    ?? throw new Exception("Record not found");

                var existingIsActive = entity.IsActive;

                _mapper.Map(command, entity);

                entity.IsActive = existingIsActive;

                OnBeforeUpdate(entity, userUUID, ipAddress);

                await _repository.UpdateAsync(entity); // ✅ IMPORTANT (missing earlier)
                if (saveChanges)
                {
                    await _repository.SaveChangesAsync();
                }
            }

            return _mapper.Map<MasterApiDto>(entity);
        }

        protected override async Task<bool> IsDuplicateAsync(MasterApiCommand command)
        {
            var apiName = command.ApiName?.Trim().ToLower();

            return await _repository.ExistsAsync(x =>
                x.ApiName != null &&
                x.ApiName.ToLower() == apiName &&
                x.UUID != command.UUID);
        }

        protected override Expression<Func<Master_Api, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.ApiName != null && x.ApiName.ToLower().Contains(searchTerm)) ||
                (x.Code != null && x.Code.ToLower().Contains(searchTerm)) ||
                (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm));
        }

        protected override Func<IQueryable<Master_Api>, IOrderedQueryable<Master_Api>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "apiname" => q => isAsc ? q.OrderBy(x => x.ApiName) : q.OrderByDescending(x => x.ApiName),
                "code" => q => isAsc ? q.OrderBy(x => x.Code) : q.OrderByDescending(x => x.Code),
                "category" => q => isAsc ? q.OrderBy(x => x.ApiCategoryUUID) : q.OrderByDescending(x => x.ApiCategoryUUID),
                "description" => q => isAsc ? q.OrderBy(x => x.ShortDescription) : q.OrderByDescending(x => x.ShortDescription),
                "displayorder" => q => isAsc ? q.OrderBy(x => x.DisplayOrder) : q.OrderByDescending(x => x.DisplayOrder),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task<List<MasterApiDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<MasterApiDto>>(entities);
        }


    }
}
