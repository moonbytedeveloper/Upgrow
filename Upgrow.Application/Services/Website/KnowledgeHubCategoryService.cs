using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class KnowledgeHubCategoryService : MasterServiceBase<Knowledge_Hub_Category, KnowledgeHubCategoryDto, KnowledgeHubCategoryCommand>, IKnowledgeHubCategoryService
    {
        public KnowledgeHubCategoryService(IMasterRepository<Knowledge_Hub_Category> repository, IMapper mapper)
          : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(KnowledgeHubCategoryCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<Knowledge_Hub_Category, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                 (x.IconImage != null && x.IconImage.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Knowledge_Hub_Category>, IOrderedQueryable<Knowledge_Hub_Category>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }

        public async Task<List<KnowledgeHubCategoryDto>> GetAllAsync()
        {
            var entities = await _repository.FindAllAsync(x=>x.IsActive == true);
            return _mapper.Map<List<KnowledgeHubCategoryDto>>(entities);
        }
    }
}
