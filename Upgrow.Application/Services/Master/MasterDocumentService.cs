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
    public class MasterDocumentService :MasterServiceBase<Master_Document, MasterDocumentDto, MasterDocumentCommand>, IMasterDocumentService
    {
        public MasterDocumentService(IMasterRepository<Master_Document> repository, IMapper mapper) : base(repository, mapper) { }
        protected override Expression<Func<Master_Document, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
            (x.Title != null && x.Title.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(MasterDocumentCommand command)
        {
            return await _repository.ExistsAsync(x => x.Title!.ToLower().Trim() == command.Title.ToLower().Trim() &&
           x.UUID != command.UUID);
        }

        protected override Func<IQueryable<Master_Document>, IOrderedQueryable<Master_Document>>? BuildSortExpression(
           string? sortColumn,
           string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "title" => q => isAsc ? q.OrderBy(x => x.Title) : q.OrderByDescending(x => x.Title),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
    }
}
