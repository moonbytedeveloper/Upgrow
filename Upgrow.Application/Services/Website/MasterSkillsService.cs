using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.Website;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.Website;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.Website;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Website;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Website
{
    public class MasterSkillsService : MasterServiceBase<Master_Skills, MasterSkillsDto, MasterSkillsCommand>, IMasterSkillsService
    {
        public MasterSkillsService(IMasterRepository<Master_Skills> repository, IMapper mapper)
         : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterSkillsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }

        // Define which fields to search
        protected override Expression<Func<Master_Skills, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_Skills>, IOrderedQueryable<Master_Skills>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }




    }
}
    
