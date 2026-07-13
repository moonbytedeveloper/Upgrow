using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class ApiXCategoryService : MasterServiceBase<ApiXCategory, ApiXCategoryDto, ApiXCategoryCommand>, IApiXCategoryService
    {
        public ApiXCategoryService(IMasterRepository<ApiXCategory> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Duplicate check: Title must be unique (case-insensitive, trimmed)
        protected override async Task<bool> IsDuplicateAsync(ApiXCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        public async Task<List<ApiXCategoryDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllActiveAsync();
            return _mapper.Map<List<ApiXCategoryDto>>(entities);
        }

        public async Task<List<ApiXCategoryDto>> GetByCategoryUUIDAsync(string categoryUUID)
        {
            var entities = await _repository.FindAllAsync(x => x.CategoryUUID == categoryUUID && x.IsActive);
            return _mapper.Map<List<ApiXCategoryDto>>(entities.OrderBy(x => x.SequenceNo));
        }

        protected override Expression<Func<ApiXCategory, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }
        

        protected override Func<IQueryable<ApiXCategory>, IOrderedQueryable<ApiXCategory>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "sequenceno" => q => isAsc ? q.OrderBy(x => x.SequenceNo) : q.OrderByDescending(x => x.SequenceNo),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}