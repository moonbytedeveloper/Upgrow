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
    public class CertificationsService : MasterServiceBase<Certifications, CertificationsDto, CertificationsCommand>, ICerificationsService
    {
        public CertificationsService(IMasterRepository<Certifications> repository, IMapper mapper)
          : base(repository, mapper) { }

        protected override async Task<bool> IsDuplicateAsync(CertificationsCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.Name!.ToLower().Trim() == command.Name.ToLower().Trim() &&
                x.UUID != command.UUID);
        }
        // Define which fields to search
        protected override Expression<Func<Certifications, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.Name != null && x.Name.ToLower().Contains(searchTerm)) ||
                 (x.FilePath != null && x.FilePath.ToLower().Contains(searchTerm));

        }
        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Certifications>, IOrderedQueryable<Certifications>>? BuildSortExpression(
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
