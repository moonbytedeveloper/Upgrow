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
using Upgrow.Domain.Entities;
using Upgrow.Domain.IRepositories;

namespace Upgrow.Application.Services.Master
{
    public class MasterCareerService : MasterServiceBase<Master_Career, MasterCareerDto, MasterCareerCommand>, IMasterCareerService
    {
        public MasterCareerService(IMasterRepository<Master_Career> repository, IMapper mapper)
          : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(MasterCareerCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.Experience!.ToLower().Trim() == command.Experience.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<Master_Career, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                (x.Experience != null && x.Experience.ToLower().Contains(searchTerm)) ||
                (x.DepartmentUUID != null && x.DepartmentUUID.ToLower().Contains(searchTerm)) ||
                (x.ShortDescription != null && x.ShortDescription.ToLower().Contains(searchTerm)) ||
                (x.IconImage != null && x.IconImage.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_Career>, IOrderedQueryable<Master_Career>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "name" => q => isAsc ? q.OrderBy(x => x.Name) : q.OrderByDescending(x => x.Name),
                "department" => q => isAsc ? q.OrderBy(x => x.DepartmentUUID) : q.OrderByDescending(x => x.DepartmentUUID),
                "noofposition" => q => isAsc ? q.OrderBy(x => x.NumberOfPosition) : q.OrderByDescending(x => x.NumberOfPosition),
                "experiense" => q => isAsc ? q.OrderBy(x => x.Experience) : q.OrderByDescending(x => x.Experience),
                "shortdescription" => q => isAsc ? q.OrderBy(x => x.ShortDescription) : q.OrderByDescending(x => x.ShortDescription),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };


        }
    }
}
