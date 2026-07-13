using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.DropDown;
using Upgrow.Application.DTOs;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Common;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public abstract class MasterServiceBase<TEntity, TDto, TCommand> : IMasterService<TDto, TCommand>
     where TEntity : class, IMasterEntity, new()
     where TDto : class
     where TCommand : class, IMasterCommand
    {
        protected readonly IMasterRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        /// <summary>
        /// Display name for error messages (e.g., "Gender", "Country")
        /// </summary>
        protected virtual string EntityDisplayName => typeof(TEntity).Name.Replace("Master_", "");

        protected MasterServiceBase(IMasterRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #region CRUD Operations (Override if needed)

        public virtual async Task<TDto?> GetByUuidAsync(string uuid)
        {
            if (string.IsNullOrWhiteSpace(uuid))
                return null;

            var entity = await _repository.GetByUuidAsync(uuid);
            return entity == null ? null : _mapper.Map<TDto>(entity);
        }

        public virtual Task<PagedResult<TDto>> GetPagedAsync(DataTableRequest request)
    => GetPagedAsync(request, queryModifier: null);

        protected async Task<PagedResult<TDto>> GetPagedAsync(
            DataTableRequest request,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier)
        {
            Expression<Func<TEntity, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                filter = BuildSearchFilter(request.Search.Trim().ToLower());
            }

            var orderBy = BuildSortExpression(request.SortColumn, request.SortDirection);

            var result = await _repository.GetPagedAsync(
                filter,
                request,
                orderBy,
                queryModifier);

            return new PagedResult<TDto>
            {
                Items = _mapper.Map<List<TDto>>(result.Items),
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
        }

        public virtual async Task SaveAsync(TCommand command, string userUuid, string ip)
        {
            // Custom validation (override ValidateAsync for custom rules)
            await ValidateAsync(command);

            // Duplicate check
            if (await IsDuplicateAsync(command))
            {
                throw new Exception($"{EntityDisplayName} already exists.");
            }

            if (string.IsNullOrEmpty(command.UUID))
            {
                await CreateAsync(command, userUuid, ip);
            }
            else
            {
                await UpdateAsync(command, userUuid, ip);
            }
        }

        public virtual async Task SaveAsync(TCommand command, string userUuid, string ip, bool saveChanges)
        {
            await ValidateAsync(command);

            if (await IsDuplicateAsync(command))
                throw new Exception(
                    $"{EntityDisplayName} already exists.");

            if (string.IsNullOrEmpty(command.UUID))
            {
                await CreateAsync(
                    command,
                    userUuid,
                    ip,
                    saveChanges);
            }
            else
            {
                await UpdateAsync(
                    command,
                    userUuid,
                    ip,
                    saveChanges);
            }
        }

        public virtual async Task DeleteAsync(string uuid, string userUuid, string ip)
        {
            var entity = await _repository.GetByUuidAsync(uuid)
                ?? throw new Exception("Record not found");

            entity.IsActive = false;
            await _repository.UpdateAsync(entity);
        }

        /// <summary>
        /// Toggles the IsActive status and returns the new status
        /// </summary>
        public virtual async Task<bool> ToggleActiveAsync(string uuid, string userUuid, string ip)
        {
            var entity = await _repository.GetByUuidAsync(uuid)
                ?? throw new Exception("Record not found");

            entity.IsActive = !entity.IsActive;
            await _repository.UpdateAsync(entity);

            return entity.IsActive;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }
        #endregion

        #region Protected Methods (Override for customization)

        /// <summary>
        /// Override to add custom validation logic
        /// </summary>
        protected virtual Task ValidateAsync(TCommand command)
        {
            // Default: no additional validation
            return Task.CompletedTask;
        }

        /// <summary>
        /// Override to define duplicate check logic for your entity
        /// </summary>
        protected abstract Task<bool> IsDuplicateAsync(TCommand command);

        /// <summary>
        /// Override to define which properties to search
        /// </summary>
        protected abstract Expression<Func<TEntity, bool>>? BuildSearchFilter(string searchTerm);

        /// <summary>
        /// Override to define sorting logic
        /// Default: sort by Id
        /// </summary>
        protected virtual Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";
            return q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id);
        }

        protected virtual async Task CreateAsync(TCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            var entity = _mapper.Map<TEntity>(command);
            entity.UUID = Utils.GetUUID();
            entity.IsActive = true;

            command.UUID = entity.UUID;
            // Override OnBeforeCreate for additional properties
            OnBeforeCreate(entity, userUuid, ip);

            await _repository.AddAsync(entity, saveChanges);
        }

        protected virtual async Task UpdateAsync(TCommand command, string userUuid, string ip, bool saveChanges = true)
        {
            var entity = await _repository.GetByUuidAsync(command.UUID!) ?? throw new Exception("Record not found");


            _mapper.Map(command, entity);

            // Override OnBeforeUpdate for additional properties
            OnBeforeUpdate(entity, userUuid, ip);

            await _repository.UpdateAsync(entity, saveChanges);
        }

        /// <summary>
        /// Override to set additional properties before create (e.g., CreatedBy, CreatedAt)
        /// </summary>
        protected virtual void OnBeforeCreate(TEntity entity, string userUuid, string ip) { }

        /// <summary>
        /// Override to set additional properties before update (e.g., UpdatedBy, UpdatedAt)
        /// </summary>
        protected virtual void OnBeforeUpdate(TEntity entity, string userUuid, string ip) { }

        public async Task<List<MasterDropDownDto>> GetDropdownAsync(Func<TCommand, string> displaySelector)
        {
            var entities = await _repository.GetAllActiveAsync();

            return entities
                .Select(e => _mapper.Map<TCommand>(e))
                .Select(c => new MasterDropDownDto
                {
                    UUID = c.UUID!,
                    Title = displaySelector(c)   // use the lambda to get the display property
                })
                .OrderBy(e => e.Title)
                .ToList();
        }


        #endregion
    }
}
