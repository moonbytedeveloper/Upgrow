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
    public class OurTeamService : MasterServiceBase<OurTeam, OurTeamDto, OurTeamCommand>, IOurTeamService
    {
        public OurTeamService(IMasterRepository<OurTeam> repository, IMapper mapper)
          : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(OurTeamCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.Designation!.ToLower().Trim() == command.Description.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<OurTeam, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm)) ||
                (x.Designation != null && x.Designation.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<OurTeam>, IOrderedQueryable<OurTeam>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "description" => q => isAsc ? q.OrderBy(x => x.Description) : q.OrderByDescending(x => x.Description),
                "designation" => q => isAsc ? q.OrderBy(x => x.Designation) : q.OrderByDescending(x => x.Designation),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }
    }
}
