using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Commands.Master;
using VerifyIndia.Application.DTO.Master;
using VerifyIndia.Application.IServices.Master;
using VerifyIndia.Domain.Common;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Domain.IRepositories;

namespace VerifyIndia.Application.Services.Master
{
    public class MasterEmailTemplateService : MasterServiceBase<Master_EmailTemplate, MasterEmailTemplateDto, MasterEmailTemplateCommand>, IMasterEmailTemplateService
    {
        public MasterEmailTemplateService(IMasterRepository<Master_EmailTemplate> repository, IMapper mapper)
          : base(repository, mapper) { }

        // Define which field to check for duplicates
        protected override async Task<bool> IsDuplicateAsync(MasterEmailTemplateCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.EmailTemplateName!.ToLower().Trim() == command.EmailTemplateName.ToLower().Trim() &&
                x.UUID != command.UUID);
        }



        // Define which fields to search
        protected override Expression<Func<Master_EmailTemplate, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return x =>
                (x.EmailTemplateName != null && x.EmailTemplateName.ToLower().Contains(searchTerm)) ||
                 (x.EmailSubject != null && x.EmailSubject.ToLower().Contains(searchTerm));
        }

        // Define sorting (optional - remove if default Id sorting is fine)
        protected override Func<IQueryable<Master_EmailTemplate>, IOrderedQueryable<Master_EmailTemplate>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "templatename" => q => isAsc ? q.OrderBy(x => x.EmailTemplateName) : q.OrderByDescending(x => x.EmailTemplateName),
                "subject" => q => isAsc ? q.OrderBy(x => x.EmailSubject) : q.OrderByDescending(x => x.EmailSubject),

                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }

        public async Task<MasterEmailTemplateDto?> GetByIdAsync(decimal templateId)
        {
            var paged = await _repository.GetPagedAsync(
                filter: x => x.Id == templateId,
                pagination: new PaginationParams { PageNumber = 1, PageSize = 1 });

            var entity = paged.Items.FirstOrDefault();
            return entity == null ? null : _mapper.Map<MasterEmailTemplateDto>(entity);
        }
    }
}
    
