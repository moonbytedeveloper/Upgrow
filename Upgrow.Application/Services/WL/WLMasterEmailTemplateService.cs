using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Application.Commands.WL;
using Upgrow.Application.DTO.Master;
using Upgrow.Application.DTO.WL;
using Upgrow.Application.IServices.WL;
using Upgrow.Domain.Common;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.WL;
using Upgrow.Domain.IRepositories;
using Upgrow.Domain.IRepositories.WL;

namespace Upgrow.Application.Services.WL
{
    public class WLMasterEmailTemplateService
    : WLBaseService<WL_MasterEmailTemplate, WLMasterEmailTemplateDto, WLMasterEmailTemplateCommand>,
      IWLMasterEmailTemplateService
    {
        private readonly IWLMasterEmailTemplateRepository _emailTemplateRepository;
        public WLMasterEmailTemplateService(
            IMasterRepository<WL_MasterEmailTemplate> repository,
            IWLMasterEmailTemplateRepository emailTemplateRepository,
            IMasterRepository<Tenant> tenantRepository,
            IMapper mapper)
            : base(repository, tenantRepository, mapper)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        protected override string EntityDisplayName => "Email Template";

        protected override Expression<Func<WL_MasterEmailTemplate, bool>>? BuildSearchFilter(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return x =>
                (x.EmailTemplateName != null && x.EmailTemplateName.ToLower().Contains(searchTerm)) ||
                (x.EmailSubject != null && x.EmailSubject.ToLower().Contains(searchTerm)) ||
                (x.TenantName != null && x.TenantName.ToLower().Contains(searchTerm)) ||
                (x.Description != null && x.Description.ToLower().Contains(searchTerm));
        }

        protected override async Task<bool> IsDuplicateAsync(WLMasterEmailTemplateCommand command)
        {
            return await _repository.ExistsAsync(x =>
                x.EmailTemplateName!.ToLower().Trim() == command.EmailTemplateName.ToLower().Trim() &&
                x.TenantId == command.TenantId &&
                x.UUID != command.UUID);
        }

        protected override Func<IQueryable<WL_MasterEmailTemplate>, IOrderedQueryable<WL_MasterEmailTemplate>>? BuildSortExpression(
            string? sortColumn,
            string? sortDirection)
        {
            var isAsc = sortDirection?.ToLower() == "asc";

            return sortColumn?.ToLower() switch
            {
                "emailtemplatename" => q => isAsc
                    ? q.OrderBy(x => x.EmailTemplateName)
                    : q.OrderByDescending(x => x.EmailTemplateName),
                "emailsubject" => q => isAsc
                    ? q.OrderBy(x => x.EmailSubject)
                    : q.OrderByDescending(x => x.EmailSubject),
                "tenantname" => q => isAsc
                    ? q.OrderBy(x => x.TenantName)
                    : q.OrderByDescending(x => x.TenantName),
                _ => q => isAsc ? q.OrderBy(x => x.Id) : q.OrderByDescending(x => x.Id)
            };
        }
        public async Task<WLMasterEmailTemplateDto?> GetByIdAsync(decimal templateId)
        {
            var entity = await _emailTemplateRepository.GetByIdAsync(templateId);
            return _mapper.Map<WLMasterEmailTemplateDto?>(entity);
        }
    }
}
