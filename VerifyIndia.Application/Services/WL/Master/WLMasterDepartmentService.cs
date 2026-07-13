using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.Master;
using Upgrow.Application.Commands.WL.Master;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL.Master;
using Upgrow.Application.IServices.Master;
using Upgrow.Application.IServices.WL.Master;
using Upgrow.Application.Services.Master;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL.Master;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.WL.Master
{
    public class WLMasterDepartmentService : MasterServiceBase<WL_MasterDepartment, WLMasterDepartmentDto, WLMasterDepartmentCommand>, IWLMasterDepartmentService
    {
        public WLMasterDepartmentService(IMasterRepository<WL_MasterDepartment> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override Expression<Func<WL_MasterDepartment, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.Title != null && x.Title.ToLower().Contains(searchTerm)) ||
                (x.ShortTitle != null && x.ShortTitle.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterDepartmentCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        protected override Func<IQueryable<WL_MasterDepartment>, IOrderedQueryable<WL_MasterDepartment>>? BuildSortExpression(
       string? sortColumn,
       string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                "shortname" => q => isAsc ? q.OrderBy(x => x.ShortTitle) : q.OrderByDescending(x => x.ShortTitle),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

    }
}

