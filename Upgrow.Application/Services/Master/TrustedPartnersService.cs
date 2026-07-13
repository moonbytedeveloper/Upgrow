using AutoMapper;
using System;
using System.Collections.Generic;
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
    public class TrustedPartnersService :  MasterServiceBase<TrustedPartners, TrustedPartnersDto, TrustedPartnersCommand>, ITrustedPartnersService
    {
        public TrustedPartnersService(IMasterRepository<TrustedPartners> repository, IMapper mapper)
         : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(TrustedPartnersCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<TrustedPartners, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                 (x.IconImage != null && x.IconImage.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<TrustedPartners>, IOrderedQueryable<TrustedPartners>>? BuildSortExpression(
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
    }
}
